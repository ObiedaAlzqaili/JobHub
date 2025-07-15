using System.Diagnostics;
using JobHub.Data;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger ,ApplicationDbContext context )
        {
            _context = context;
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View();
        }


        public IActionResult CreateEndUserAccount()
        {
            ViewBag.SkillsLevels = _context.SkillLevels.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
