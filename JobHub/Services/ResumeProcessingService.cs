using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using JobHub.Data;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Path = System.IO.Path;

namespace JobHub.Services
{
    public class ResumeProcessingService : IResumeProcessingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOpenAiKeywordExtraction _keywordExtractor;
        private readonly IFileStorageService _fileStorage;

        public ResumeProcessingService(
            ApplicationDbContext context,
            IOpenAiKeywordExtraction keywordExtractor,
            IFileStorageService fileStorage)
        {
            _context = context;
            _keywordExtractor = keywordExtractor;
            _fileStorage = fileStorage;
        }

        public async Task<Resume> ProcessUploadedResumeAsync(Stream fileStream, string fileName, string userId)
        {
            // 1. Convert file to base64 and get file type
            string base64Content;
            using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                base64Content = Convert.ToBase64String(memoryStream.ToArray());
            }

            // Get file type (extension)
            var fileType = Path.GetExtension(fileName).ToLowerInvariant();

            // Reset stream position for text extraction
            fileStream.Position = 0;

            // 2. Extract text based on file type
            string resumeText;
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                resumeText = ExtractTextFromPdf(fileStream);
            }
            else if (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                resumeText = ExtractTextFromDocx(fileStream);
            }
            else if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = new StreamReader(fileStream);
                resumeText = await reader.ReadToEndAsync();
            }
            else
            {
                throw new NotSupportedException("File format not supported. Please upload a PDF, DOCX, or TXT file.");
            }

            // 3. Extract AI keywords
            var aiKeywords = await _keywordExtractor.ExtractKeywordsFromResumeAsync(resumeText);

            // 4. Create and save resume record
            var resume = new Resume
            {
                ExtractedText = resumeText,
                UserId = userId,
                FileContentBase64 = base64Content,
                FileName = fileName,
                FileType = fileType,
                UploadedAt = DateTime.UtcNow,
                AiKeywords = aiKeywords
            };

            try
            {
                _context.Resumes.Add(resume);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving resume to database: " + ex.Message, ex);
            }

            return resume;
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            try
            {
                using (var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(pdfStream))
                {
                    var text = new StringBuilder();
                    foreach (var page in pdfDocument.GetPages())
                    {
                        text.AppendLine(page.Text);
                    }
                    return text.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to extract text from PDF: " + ex.Message, ex);
            }
        }

        private string ExtractTextFromDocx(Stream docxStream)
        {
            try
            {
                // Create a new MemoryStream to work with because WordprocessingDocument may close the original stream
                using var memoryStream = new MemoryStream();
                docxStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                using var document = WordprocessingDocument.Open(memoryStream, false);
                var body = document.MainDocumentPart?.Document.Body;
                if (body == null) return string.Empty;

                return string.Join(Environment.NewLine, body.Descendants<Paragraph>()
                    .Select(p => p.InnerText));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to extract text from DOCX: " + ex.Message, ex);
            }
        }
    }
}