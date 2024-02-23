using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
namespace MVC.Controllers
{
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityRepositories _cityrepo;

        public CityController(ILogger<CityController> logger, ICityRepositories cityrepo)
        {
            _logger = logger;
            _cityrepo = cityrepo;
        }

        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }
            ViewBag.IsAuthenticated = true;
            var allCity = _cityrepo.FetchAllcity();
            return View(allCity);
        }

        public IActionResult AddCity()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }
            ViewBag.IsAuthenticated = true;
            var states = _cityrepo.GetAllstate();
            ViewBag.states = new SelectList(states, "c_stateid", "c_statename");

            return View();
        }

        [HttpPost]
        public IActionResult AddCity(City city, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "No image uploaded.");
                var states = _cityrepo.GetAllstate();
                ViewBag.states = new SelectList(states, "c_stateid", "c_statename");
                return View(city);
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var imageUrl = Path.Combine("/images", fileName);
            city.c_city_photo = imageUrl;

            if (_cityrepo.AddCity(city))
            {
                return Ok(new { success = true });
            }

            var states2 = _cityrepo.GetAllstate();
            ViewBag.states = new SelectList(states2, "c_stateid", "c_statename");
            return BadRequest(new { error = "Failed to add city." });
        }

        public IActionResult GetStates()
        {
            var states = _cityrepo.GetAllstate();
            return Json(states);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}