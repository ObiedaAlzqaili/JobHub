namespace JobHub.Models
{
    public class Resume
    {
        public int Id { get; set; }

        public string FileContentBase64 { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string ExtractedText { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UserId { get; set; }
        public EndUser? User { get; set; }
        public string? AiKeywords { get; set; }


        public ICollection<JobApplication> JobApplications { get; set; }
    }

}