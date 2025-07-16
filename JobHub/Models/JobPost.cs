namespace JobHub.Models
{
   
        public class JobPost
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string RequiredSkills { get; set; }
            public string? AiKeyWords { get; set; }
            public DateTime PostedAt { get; set; }

            public string CompanyId { get; set; }
            public Company Company { get; set; }

           
        }

    
}