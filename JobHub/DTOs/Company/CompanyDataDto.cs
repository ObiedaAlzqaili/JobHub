using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobHub.Models;
using Microsoft.AspNetCore.Http;

namespace JobHub.DTOs.Company
{
    public class CompanyDataDto
    {
        // Company Information
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Company description is required")]
        public string CompanyDescription { get; set; }

        public IFormFile CompanyLogo { get; set; }
        public string? CompanyLogoBase64 { get; set; }
        public string? CompanyLogoName { get; set; }
        public string? CompanyLogoType { get; set; }

        // Contact Person Information
        [Required(ErrorMessage = "Contact person name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string DayOfBirth { get; set; }
        public string Description { get; set; }

        // Personal Image
        public IFormFile PersonalImage { get; set; }
        public string? PersonalImageBase64 { get; set; }
        public string? PersonalImageName { get; set; }
        public string? PersonalImageType { get; set; }

        // Job Posts (if needed for initialization)
        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
    }
}