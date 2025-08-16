namespace JobHub.Models
{
    public class Company : Person
    {
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
       
        public ICollection<JobPost>? JobPosts { get; set; }
    }
   
}
