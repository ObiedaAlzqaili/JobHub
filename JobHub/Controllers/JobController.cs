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
        public IActionResult JobPostApplication(JobApplicationDto jobApplication)
        {
            if (ModelState.IsValid)
            {
                _jobRepository.CreateJobApplication(jobApplication);
                return RedirectToAction("Index", "Home");
            }
            return View(jobApplication);
        }

    } 
}
