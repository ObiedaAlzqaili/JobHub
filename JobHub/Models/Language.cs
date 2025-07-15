namespace JobHub.Models
{
    public class Language
    {

        public int Id { get; set; }

        public string UserId { get; set; }
        public EndUser? User { get; set; }
        public string Name { get; set; }

        public int SkillLevelId { get; set; }
        public SkillLevel? SkillLevel { get; set; } 

    }
}