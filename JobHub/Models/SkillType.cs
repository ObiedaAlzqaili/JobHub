namespace JobHub.Models
{
    public class SkillType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Skill>? Skills { get; set; }
    }

}