using Microsoft.AspNetCore.Builder;

namespace JobHub.Models
{
   
        public class JobPost
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string RequiredSkills { get; set; }
            public string companyName { get; set; }

           public string? ImageCompanyBase64 { get; set; }
            public string? ImageCompanyType { get; set; }
            public string? ImageCompanyName { get; set; }
            public string  Location { get; set; }
            public string? AiKeyWords { get; set; }
            public DateTime PostedAt { get; set; }

            public string CompanyId { get; set; }
            public Company Company { get; set; }
             public ICollection<JobApplication> JobApplications { get; set; }

    }

    
}