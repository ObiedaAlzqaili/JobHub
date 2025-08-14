using JobHub.Data;

using JobHub.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Controllers
{
    //[Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            //var company = _context.Companies
            //    .FirstOrDefault(c => c.Id == "fsdgviolhjfdsio");

            //var model = new ProfileViewDto
            //{
            //    CompanyName = company.CompanyName,
            //    FullName = company.FullName,
           
            //};

            return View();
        }

        public IActionResult JobPosts()
        {
            //var jobPosts = _context.JobPosts
            //    .Where(j => j.CompanyId == "ukioswhdefoh")
            //    .Include(j => j.JobApplications)
            //    .ToList();

            //var model = new JobPostViewDto
            //{
            //    JobPosts = jobPosts,
            //    CompanyName = "fayez"
            //};

            return View();
        }

        //[HttpPost]
        //public IActionResult CreateJobPost()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var jobPost = new JobPost
        //        {
        //            Title = model.Title,
        //            Description = model.Description,
        //            // Map other properties
        //            CompanyId = "sduikhdoicu"
        //        };

        //        _context.JobPosts.Add(jobPost);
        //        _context.SaveChanges();

        //        return RedirectToAction("JobPosts");
        //    }

        //    return PartialView("_CreateJobPostModal", model);
        //}

        public IActionResult GetApplicants(int jobPostId)
        {
            //var applicants = _context.JobApplications
            //    .Where(a => a.JobPostId == jobPostId)
            //    .Include(a => a.)
            //    .ToList();

            //var jobPost = _context.JobPosts.Find(jobPostId);

            //var model = new ApplicantsViewModel
            //{
            //    JobTitle = jobPost.Title,
            //    Applicants = applicants
            //};

            //return PartialView("_ApplicantsModal", model);
            return PartialView();
        }
    }
}