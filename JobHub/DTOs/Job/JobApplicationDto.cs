namespace JobHub.DTOs.Job
{
    public class JobApplicationDto
    {
      
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ResumeBase64 { get; set; }
        public string ResumeFileName { get; set; }

        public string ResumeFileType { get; set; }
        public DateTime ApplicationDate { get; set;}
        public int JobPostId { get; set; }

        
    }
}
