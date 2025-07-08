namespace JobHub.Models
{
    public class MatchResult
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }

        public int JobPostId { get; set; }
        public JobPost? JobPost { get; set; }

        public float MatchScore { get; set; }
        public string? AiExplanation { get; set; }
    }

}