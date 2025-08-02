using System.Diagnostics;
using System.Security.Claims;
using JobHub.Data;
using JobHub.DTOs.UserAccount;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IProfileReposotity _repo;

        public HomeController(ILogger<HomeController> logger ,ApplicationDbContext context, IProfileReposotity repo)
        {
            _context = context;
            _logger = logger;
            _repo = repo;
        }
       
        public IActionResult Index()
        {

            return View();
        }
        

        [HttpGet]
        public IActionResult CreateEndUserAccount()
        {
            ViewBag.SkillsLevels = _context.SkillLevels.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEndUserAccount(UserDataDto userDto)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    // Map DTO to EndUser model
                    var endUser = new EndUser
                    {
                        Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        FullName = userDto.FullName,
                        PhoneNumber = userDto.PhoneNumber,
                        Address = userDto.Address,
                        DayOfBirth = DateTime.Parse(userDto.DayOfBirth).ToString(),
                        Description = userDto.Description,
                        PersonalImageBase64 = userDto.PersonalImageBase64,
                        PersonalImageType = userDto.PersonalImageType,
                        PersonalImageName = userDto.PersonalImageName,
                        EducationList = userDto.Education.Select(e => new Education
                        {
                            CollegeName = e.CollegeName,
                            Description = e.Description,
                            StartDate = e.StartDate,
                            EndDate = e.EndDate,
                            Gpa = e.Gpa
                        }).ToList(),
                        ExperienceList = userDto.Experiences.Select(e => new Experience
                        {
                            CompanyName = e.CompanyName,
                            Title = e.Title,
                            Location = e.Location,
                            StartDate = e.StartDate,
                            EndDate = e.EndDate,
                            Description = e.Description
                        }).ToList(),
                        Languages = userDto.Languages.Select(l => new Language
                        {
                            Name = l.LanguageName,
                            SkillLevelId = l.LanguageLevel,
                        }).ToList(),
                    };
                   
                    await _repo.UpdateEndUserAsync(endUser);

                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "An error occurred while creating the account. Please try again.");
                return View(userDto);
            }
           return View(userDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
