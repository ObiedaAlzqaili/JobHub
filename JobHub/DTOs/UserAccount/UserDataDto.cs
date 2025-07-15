namespace JobHub.DTOs.UserAccount
{
    public class UserDataDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string DayOfBirth { get; set; }
        public string? Description { get; set; }
        public string? PersonalImageBase64 { get; set; }
        public string PersonalImageType { get;set; }
        public string PersonalImageName { get; set; }

        public string HeadLine { get; set; }

        public string CollegeName { get; set; }
        public string EducationDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Gpa { get; set; }

        public List<ExperinceDto> Experiences { get; set; } = new List<ExperinceDto>();

        public List<EducationDto> Education { get; set; } = new List<EducationDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();

        public string ResumeBase64 { get; set; }
        public string ResumeType { get; set; }
        public string ResumeName { get; set; }

    }

}
