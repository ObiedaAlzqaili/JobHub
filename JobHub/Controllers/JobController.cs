using JobHub.DTOs.Job;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Models;
using JobHub.repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<IActionResult> Index()
        {
            var jobposts = await _jobRepository.GetFiveJobAsync();
            return View(jobposts);
        }
        [HttpGet]
        public IActionResult JobSearchResult(string title, string location)
        {
            var jobPosts = _jobRepository.GetJobsByTitleAsync(title, location).Result;
            return View(jobPosts);
        }

        //[HttpGet]
        //public async Task<IActionResult> JobPostDetails(int Id)
        //{
        //    var jobPost =   _jobRepository.GetJobByIdAsync(Id);

        //    return View(jobPost);
        //}
        [HttpGet]
        public async Task<IActionResult> JobPostDetails(int Id)
        {
            

            // Dummy data for demonstration purposes
            var dummyJobPost = new JobPost
            {
                Id = Id,
                Title = "Software Engineer",
                PostedAt = DateTime.UtcNow.AddDays(-10),
                companyName = "TechCorp Inc.",
                Description = "anything",
                RequiredSkills = "C#, ASP.NET, SQL",
                ImageCompanyBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA\nAAAAFCAIAAAACNbyblAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAA\nCwQAAwAAAAECAwQFBgcICQoLDA0ODxAREhMUFRYXGBkaGxwdHh8gISIjJCUmJygpKissLS4\n",
                
                Location = "New York, NY"
            };

          

            return View(dummyJobPost);
        }
        [HttpGet]
        public async Task<IActionResult> JobPostApplication(int Id)
        {
            var jobPost =  _jobRepository.GetJobByIdAsync(Id);
            var jobApplication = new JobApplicationDto
            {
                JobPostId = Id,
                ApplicationDate = DateTime.UtcNow,
                ApplicantName = string.Empty, // Placeholder, to be filled by the user                                                  
                ApplicantEmail = string.Empty, 
                PhoneNumber = string.Empty,                              
                ResumeBase64 = string.Empty,
                ResumeFileName = string.Empty, 
                ResumeFileType = string.Empty,
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JobPostApplication(string FullName,string Email, string Phone , IFormFile Resume , int companyId)
        {
            if (ModelState.IsValid)
            {
                string resumeBase64 = null;
                string resumeType = null;
                string resumeName = null;

                if (Resume != null && Resume.Length > 0)
                {
                    var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    using (var ms = new MemoryStream())
                    {
                        await Resume.CopyToAsync(ms);
                        resumeBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    resumeType = Resume.ContentType;
                    resumeName = Resume.FileName;
                }
                   


                var jobApplication = new JobApplicationDto
                {
                    JobPostId = companyId,
                    EndUserId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                    
                    ApplicantName = FullName,
                    ApplicantEmail = Email,
                    PhoneNumber = Phone,
                    ApplicationDate = DateTime.UtcNow,
                    ResumeBase64 = resumeBase64,
                    ResumeFileName = resumeName,
                    ResumeFileType = resumeType
                };
                _jobRepository.CreateJobApplication(jobApplication);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

    } 
}
