﻿using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
