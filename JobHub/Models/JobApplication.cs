namespace JobHub.Models
{
    public class JobApplication
        {

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string? CoverLetter { get; set; }
        public string? ResumeBase64 { get; set; }
        public string? ResumeType { get; set; }
        public string? ResumeName { get; set; }
        public string EndUserId { get; set; }
        public EndUser EndUser { get; set; }

        public int JobPostId { get; set; }
        public JobPost JobPost { get; set; }

        public DateTime AppliedOn { get; set; }
        public string Status { get; set; } 
        
    }
}