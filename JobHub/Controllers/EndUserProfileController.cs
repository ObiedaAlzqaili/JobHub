using JobHub.DTOs.UserAccount;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    [Authorize]
    public class EndUserProfileController : Controller
    {
        private readonly IProfileReposotity _profileRepository;

        public EndUserProfileController(IProfileReposotity profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<IActionResult> Index()
        {

            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var endUserProfile = await _profileRepository.GetEndUserByIdAsync(userId);
            return View(endUserProfile);
        }

        [HttpPost]
        public IActionResult UpdateEndUserProfile(UserDataDto endUserProfileDto)
        {
            if (ModelState.IsValid)
            {
                _profileRepository.UpdateEndUserAsync(new EndUser
                {
                    Id = endUserProfileDto.Id,
                    FullName = endUserProfileDto.FullName,
                    PhoneNumber = endUserProfileDto.PhoneNumber,
                    Address = endUserProfileDto.Address,
                    DayOfBirth = DateTime.Parse(endUserProfileDto.DayOfBirth).ToString(),
                    Description = endUserProfileDto.Description,
                    PersonalImageBase64 = endUserProfileDto.PersonalImageBase64,
                    PersonalImageType = endUserProfileDto.PersonalImageType,
                    PersonalImageName = endUserProfileDto.PersonalImageName,
                    EducationList = endUserProfileDto.Education.Select(e => new Education
                    {
                        CollegeName = e.CollegeName,
                        FieldOfStudy = e.FieldOfStudy,
                        Description = e.Description,
                        Gpa = e.Gpa,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    ExperienceList = endUserProfileDto.Experiences.Select(e => new Experience
                    {
                        CompanyName = e.CompanyName,
                        Description = e.Description,
                        Location = e.Location,
                        Title = e.Title,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    Skills = endUserProfileDto.Skills.Select(s => new Skill
                    {
                        SkillName = s.SkillName,
                        SkillLevel = s.SkillLevel,

                    }).ToList(),
                    Languages = endUserProfileDto.Languages.Select(l => new Language
                    {
                        Name = l.LanguageName,
                        Level = l.LanguageLevel,
                    }).ToList()
                });
                return RedirectToAction("Index", "Home");
            }
            return View(endUserProfileDto);

        }
    }
}
