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

        [HttpGet]
        public IActionResult UpdateCity(int id)
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
            Console.WriteLine("id" + id);
            var city = _cityrepo.FetchByCityId(id);

            return View(city);
        }
        [HttpPost]
        public IActionResult UpdateCity(City city, IFormFile? file = null)
        {
            var existingCity = _cityrepo.FetchByCityId(city.c_cityid);
            if (existingCity == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                city.c_city_photo = existingCity.c_city_photo;
            }
            else
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, file.FileName);
                var fileName = Path.GetFileName(file.FileName);

                if (System.IO.File.Exists(filePath))
                {
                    fileName = Guid.NewGuid().ToString() + "_" + fileName;
                    filePath = Path.Combine(folderPath, fileName);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var imageUrl = Path.Combine("/images", fileName);
                city.c_city_photo = imageUrl;
            }

            if (_cityrepo.UpdateCity(city))
            {
                return Ok();
            }

            return BadRequest(new { success = false, message = "Failed to update city" });
        }



        public IActionResult GetStates()
        {
            var states = _cityrepo.GetAllstate();
            return Json(states);
        }

        [HttpPost]
        public IActionResult delete(int id)
        {
            _cityrepo.deleteCity(id);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}