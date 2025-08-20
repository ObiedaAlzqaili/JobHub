using JobHub.DTOs;
using JobHub.DTOs.UserAccount;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{

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
            if (endUserProfile == null)
            {
                return NotFound("End user profile not found.");
            }
            var endUserProfileDto = new UserDataDto
            {
                FullName = endUserProfile.FullName,
                PhoneNumber = endUserProfile.PhoneNumber,
                Address = endUserProfile.Address,
                DayOfBirth = endUserProfile.DayOfBirth,
                Description = endUserProfile.Description,
                PersonalImageBase64 = endUserProfile.PersonalImageBase64,
                PersonalImageType = endUserProfile.PersonalImageType,
                PersonalImageName = endUserProfile.PersonalImageName,
                ResumeBase64 = endUserProfile.ResumeBase64,
                ResumeType = endUserProfile.ResumeType,
                ResumeName = endUserProfile.ResumeName,
                HeadLine = endUserProfile.Headline,
                Education = endUserProfile.EducationList.Select(e => new EducationsDto
                {
                    CollegeName = e.CollegeName,
                    FieldOfStudy = e.FieldOfStudy,
                    Description = e.Description,
                    Gpa = e.Gpa,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList(),
                Experiences = endUserProfile.ExperienceList.Select(e => new ExperinceDto
                {
                    CompanyName = e.CompanyName,
                    Description = e.Description,
                    Location = e.Location,
                    Title = e.Title,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList(),
                Skills = endUserProfile.Skills.Select(s => new SkillsDto
                {
                    SkillName = s.SkillName,
                    SkillLevel = s.SkillLevel
                }).ToList(),
                Languages = endUserProfile.Languages.Select(l => new LanguageDto
                {
                    LanguageName = l.Name,
                    LanguageLevel = l.Level
                }).ToList()
            };
            return View(endUserProfileDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEndUserProfile()
        {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var endUserProfile = await _profileRepository.GetEndUserByIdAsync(userId);
            if (endUserProfile == null)
            {
                return NotFound("End user profile not found.");
            }
            var endUserProfileDto = new UserDataDto
            {
                FullName = endUserProfile.FullName,
                PhoneNumber = endUserProfile.PhoneNumber,
                Address = endUserProfile.Address,
                DayOfBirth = endUserProfile.DayOfBirth,
                Description = endUserProfile.Description,
                PersonalImageBase64 = endUserProfile.PersonalImageBase64,
                PersonalImageType = endUserProfile.PersonalImageType,
                PersonalImageName = endUserProfile.PersonalImageName,
                ResumeBase64 = endUserProfile.ResumeBase64,
                ResumeType = endUserProfile.ResumeType,
                ResumeName = endUserProfile.ResumeName,
                HeadLine = endUserProfile.Headline,
                Education = endUserProfile.EducationList.Select(e => new EducationsDto
                {
                    CollegeName = e.CollegeName,
                    FieldOfStudy = e.FieldOfStudy,
                    Description = e.Description,
                    Gpa = e.Gpa,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList(),
                Experiences = endUserProfile.ExperienceList.Select(e => new ExperinceDto
                {
                    CompanyName = e.CompanyName,
                    Description = e.Description,
                    Location = e.Location,
                    Title = e.Title,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList(),
                Skills = endUserProfile.Skills.Select(s => new SkillsDto
                {
                    SkillName = s.SkillName,
                    SkillLevel = s.SkillLevel
                }).ToList(),
                Languages = endUserProfile.Languages.Select(l => new LanguageDto
                {
                    LanguageName = l.Name,
                    LanguageLevel = l.Level
                }).ToList()
            };
            return View(endUserProfileDto);

        }

        [HttpPost]
        public IActionResult UpdateEndUserProfile(UserDataDto endUserProfileDto)
        {
            if (ModelState.IsValid)
            {
                _profileRepository.UpdateEndUserAsync(new EndUser
                {
                    Id = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
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


        //[HttpPost]
        //public async Task<IActionResult> UploadResume(IFormFile resumeFile)
        //{
        //    if (resumeFile != null && resumeFile.Length > 0)
        //    {
        //        var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return Unauthorized();
        //        }
        //        var resumeBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(resumeFile.OpenReadStream()));
        //        var resumeType = resumeFile.ContentType;
        //        var resumeName = resumeFile.FileName;
        //        await _profileRepository.UpdateResumeAsync(userId, resumeBase64, resumeType, resumeName);
        //        return RedirectToAction("Index");
        //    }
        //    return BadRequest("No file uploaded.");
        //}
        //[HttpPost]
        //public async Task<IActionResult> UploadPersonalImage(IFormFile personalImage)
        //{
        //    if (personalImage != null && personalImage.Length > 0)
        //    {
        //        var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return Unauthorized();
        //        }
        //        var personalImageBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(personalImage.OpenReadStream()));
        //        var personalImageType = personalImage.ContentType;
        //        var personalImageName = personalImage.FileName;
        //        await _profileRepository.UpdatePersonalImageAsync(userId, personalImageBase64, personalImageType, personalImageName);
        //        return RedirectToAction("Index");
        //    }
        //    return BadRequest("No file uploaded.");
        //}

        [HttpGet]
        public  IActionResult CreateProfile(string id)
        {
          var endUserProfileDto = new UserDataDto
            {
                Education = new List<EducationsDto>(),
                Experiences = new List<ExperinceDto>(),
                Skills = new List<SkillsDto>(),
                Languages = new List<LanguageDto>()
            };
            return View(endUserProfileDto);

        }
        [HttpPost]
        public async Task<IActionResult> CreateProfile(UserDataDto endUserProfileDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                if (endUserProfileDto.PersonalImage != null && endUserProfileDto.PersonalImage.Length > 0)
                {
                    // Convert the uploaded file to Base64 string
                    using (var ms = new MemoryStream())
                    {
                        await endUserProfileDto.PersonalImage.CopyToAsync(ms);
                        endUserProfileDto.PersonalImageBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    endUserProfileDto.PersonalImageType = endUserProfileDto.PersonalImage.ContentType;
                    endUserProfileDto.PersonalImageName = endUserProfileDto.PersonalImage.FileName;
                }
                if (endUserProfileDto.ResumeFile != null && endUserProfileDto.ResumeFile.Length > 0)
                {
                    // Convert the uploaded file to Base64 string
                    using (var ms = new MemoryStream())
                    {
                        await endUserProfileDto.ResumeFile.CopyToAsync(ms);
                        endUserProfileDto.ResumeBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    endUserProfileDto.ResumeType = endUserProfileDto.ResumeFile.ContentType;
                    endUserProfileDto.ResumeName = endUserProfileDto.ResumeFile.FileName;
                }
                var endUserProfile = new EndUser
                {
                    Id = userId,
                    FullName = endUserProfileDto.FullName,
                    PhoneNumber = endUserProfileDto.PhoneNumber,
                    Headline = endUserProfileDto.HeadLine,
                    Address = endUserProfileDto.Address,
                    DayOfBirth = DateTime.Parse(endUserProfileDto.DayOfBirth).ToString(),
                    Description = endUserProfileDto.Description,
                    PersonalImageBase64 = endUserProfileDto.PersonalImageBase64,
                    PersonalImageType = endUserProfileDto.PersonalImageType,
                    PersonalImageName = endUserProfileDto.PersonalImageName,
                    ResumeBase64 = endUserProfileDto.ResumeBase64,
                    ResumeType = endUserProfileDto.ResumeType,
                    ResumeName = endUserProfileDto.ResumeName,
                    
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
                };
                await _profileRepository.CreateEndUserAsync(endUserProfile);
                return RedirectToAction("Index", "Home");
            }
            return View(endUserProfileDto);

        }
       
    }
}
