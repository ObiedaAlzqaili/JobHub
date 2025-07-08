namespace JobHub.Models
{
  
        public class SkillLevel
        {
            public int Id { get; set; }
            public string Level { get; set; } // Beginner, Intermediate, Expert

            public ICollection<Skill>? Skills { get; set; }
        }

    
}