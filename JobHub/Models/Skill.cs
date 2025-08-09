namespace JobHub.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public string UserId { get; set; }
        public EndUser? User { get; set; }
  
    
    }


}