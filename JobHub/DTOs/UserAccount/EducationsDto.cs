namespace JobHub.DTOs.UserAccount
{
    public class EducationsDto
    {
        public string? CollegeName { get; set; }
        public string? FieldOfStudy { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Gpa { get; set; }
    }
}
