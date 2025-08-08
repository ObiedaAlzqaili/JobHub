using JobHub.DTOs.Job;
using JobHub.Models;
using JobHub.repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class JobController : Controller
    {
        private readonly JobRepository _jobRepository;

        public JobController(JobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult JobSearchResult(string title, string location)
        {
            var jobPosts = _jobRepository.GetJobsByTitleAsync(title, location).Result;
            return View(jobPosts);
        }

        [HttpGet]
        public IActionResult JobPostDetails(int Id)
        {
            var jobPost = _jobRepository.GetJobByIdAsync(Id);
            return View(jobPost);
        }
        [HttpGet]
        public IActionResult JobPostApplication(int Id)
        {
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
