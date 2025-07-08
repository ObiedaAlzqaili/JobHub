using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
