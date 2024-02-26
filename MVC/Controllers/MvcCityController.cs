using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
namespace MVC.Controllers
{
    public class MvcCityController : Controller
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityRepositories _cityrepo;

        public MvcCityController(ILogger<CityController> logger, ICityRepositories cityrepo)
        {
            _logger = logger;
            _cityrepo = cityrepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = true;
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}