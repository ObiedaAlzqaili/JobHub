using JobHub.Models;

namespace JobHub.DTOs.Ai
{
    public class JobMatchDto
    {
        public JobPost JobPost { get; set; }
        public int MatchPercentage { get; set; }
        public List<string> MatchedSkills { get; set; }
    }
}
