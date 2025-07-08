namespace JobHub.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public EndUser? User { get; set; }

        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}