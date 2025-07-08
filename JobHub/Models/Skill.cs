namespace JobHub.Models
{
    public class Skill
    {
        public int Id { get; set; }

        public string SkillName { get; set; }
        public string UserId { get; set; }
        public EndUser? User { get; set; }

        public int SkillTypeId { get; set; }
        public SkillType? SkillType { get; set; }

        public int SkillLevelId { get; set; }
        public SkillLevel? SkillLevel { get; set; }
    }


}