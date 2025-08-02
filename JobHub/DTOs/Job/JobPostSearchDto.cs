namespace JobHub.DTOs.Job
{
    public class JobPostSearchDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedAt { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
    }
}