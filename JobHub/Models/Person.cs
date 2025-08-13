using Microsoft.AspNetCore.Identity;

namespace JobHub.Models
{
    public class Person : IdentityUser
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? DayOfBirth { get; set; }
        public string? Description { get; set; }   
        public string? PersonalImageBase64 { get; set; }
        public string? PersonalImageName { get; set; }
        public string? PersonalImageType { get; set; }
        
       
        

        
    }
}
