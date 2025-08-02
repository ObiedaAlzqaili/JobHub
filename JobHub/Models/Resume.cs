namespace JobHub.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string ExtractedText { get; set; }
        public string AnalysisResult { get; set; }
        public DateTime UploadedAt { get; set; }

        public string UserId { get; set; }
        public EndUser? User { get; set; }

    }

}