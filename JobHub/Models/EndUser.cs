namespace JobHub.Models
{
    public class EndUser : Person
    {
        public string? Headline { get; set; }
        public string? SkillsSummary { get; set; }
        public string? AiAnalysis { get; set; }

        public ICollection<Resume>? Resumes { get; set; }
        public ICollection<Education>? EducationList { get; set; }
        public ICollection<Experience>? ExperienceList { get; set; }
        public ICollection<Skill>? Skills { get; set; }
        public ICollection<MatchResult>? MatchResults { get; set; }

        //Jobs User Applied For
        public ICollection<JobPost>? JobPosts { get; set; }
    }
}
