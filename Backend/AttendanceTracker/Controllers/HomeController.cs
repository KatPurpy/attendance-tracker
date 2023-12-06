﻿using AttendanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AttendanceTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DbCtx dbCtx;

        public HomeController(DbCtx dbCtx, ILogger<HomeController> logger)
        {
            this.dbCtx = dbCtx;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ListGroup()
        {
            ViewBag.DbCtx = dbCtx;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}