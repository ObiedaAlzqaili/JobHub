namespace JobHub.DTOs.Job
{
    public class JobPostDto
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequiredSkills { get; set; }

        public string ImageCompanyBase64 { get; set; }
        public string ImageCompanyType { get; set; }
        public string ImageCompanyName { get; set; }

        public string Location { get; set; }
        public DateTime PostedAt { get; set; }
        public string CompanyName { get; set; }
    }
}