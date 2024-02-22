using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityRepositories _cityrepo;

        public CityController(ILogger<CityController> logger , ICityRepositories cityrepo)
        {
            _logger = logger;
            _cityrepo= cityrepo;
        }

        public IActionResult Index()
        {
            var allCity = _cityrepo.FetchAllcity();
            return View(allCity);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}