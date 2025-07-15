namespace JobHub.Models
{
  
        public class Education
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public EndUser? User { get; set; }

            public string CollegeName { get; set; }
        public string FieldOfStudy { get; set; }
        public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public float Gpa { get; set; }
        

    }
}