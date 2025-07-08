namespace JobHub.Models
{
    public class Company : Person
    {
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogoBase64 { get; set; }
        public string? CompanyLogoName { get; set; }
        public string? CompanyLogoType { get; set; }
        
        public ICollection<JobPost>? JobPosts { get; set; }
    }
   
}
