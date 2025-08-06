namespace JobHub.DTOs
{
    public class MatchResultDto
    {
        public int ResumeId { get; set; }
        public int JobPostId { get; set; }
        public float MatchScore { get; set; }
        public string AiExplanation { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }


    }
}