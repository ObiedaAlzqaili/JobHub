namespace JobHub.Models
{
   
        public class JobApplication
        {

            
            public string EndUserId { get; set; }
            public EndUser EndUser { get; set; }

            public int JobPostId { get; set; }
            public JobPost JobPost { get; set; }

            public DateTime AppliedOn { get; set; }
            public string Status { get; set; } 
        
    }
}