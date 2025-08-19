using JobHub.Models;

namespace JobHub.DTOs.Ai
{
    public class ResumeMatchDto
    {
        public Resume Resume { get; set; }
        public List<JobMatchDto> MatchingJobs { get; set; }
        public int MatchScore => MatchingJobs.Any() ? (int)MatchingJobs.Average(j => j.MatchPercentage) : 0;
        public int SkillsMatched => MatchingJobs.Any() ? (int)MatchingJobs.Average(j => j.MatchedSkills.Count) : 0;
        public string ExperienceMatched => MatchingJobs.Any() ? GetExperienceLevel() : "Unknown";

        private string GetExperienceLevel()
        {
            // Simple logic to determine experience level based on job titles
            if (MatchingJobs.Any(j => j.JobPost.Title.Contains("Senior")))
                return "Senior";
            if (MatchingJobs.Any(j => j.JobPost.Title.Contains("Mid")))
                return "Mid-Level";
            return "Junior";
        }
    }
}
