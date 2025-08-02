using JobHub.DTOs.Job;

namespace JobHub.DTOs
{
    public class UserProfileDto
    {

        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Headline { get; set; }
        public string? Description { get; set; }
        
        public string? PersonalImageBase64 { get; set; }
        public string? PersonalImageName { get; set; }
        public string? PersonalImageType { get; set; }
        

      public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public List<JobPostDto> JobPosts { get; set; } = new List<JobPostDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
        public List<ResumeDto> Resumes { get; set; } = new List<ResumeDto>();
        public List<MatchResultDto> MatchResults { get; set; } = new List<MatchResultDto>();

    }
}
