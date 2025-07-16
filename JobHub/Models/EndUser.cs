namespace JobHub.Models
{
    public class EndUser : Person
    {
        public string? Headline { get; set; }
        public string? SkillsSummary { get; set; }

        public string? Gender { get; set; }

        public string? AiKeyWords { get; set; }

        public Resume? Resumes { get; set; }
        public ICollection<Education>? EducationList { get; set; }
        public ICollection<Experience>? ExperienceList { get; set; }
        public ICollection<Skill>? Skills { get; set; }
        public ICollection<MatchResult>? MatchResults { get; set; }

        public ICollection<Language>? Languages { get; set; }

        //Jobs User Applied For
        public ICollection<JobPost>? JobPosts { get; set; }
    }
}
