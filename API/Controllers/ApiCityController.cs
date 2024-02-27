
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using API.Repositories;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiCityController : ControllerBase
    {
        private readonly ICityRepositories _cityRepo;

        public ApiCityController(ICityRepositories cityRepo)
        {
            _cityRepo = cityRepo;
        }

        [HttpGet("GetCities")]
        public IActionResult GetCities(int id)
        {
            HttpContext.Session.SetInt32("userid", id);
            List<City> cities = _cityRepo.FetchAllcity();
            return Ok(cities);
        }

        [HttpGet("GetOneCity")]
        public IActionResult GetCityDetails(int id)
        {
            var CityDetail = _cityRepo.FetchByCityId(id);
            return Ok(CityDetail);
        }

        [HttpGet("states")]
        public IActionResult GetStates()
        {
            var states = _cityRepo.GetAllstate();
            return Ok(states);
        }

        [HttpPost]
        public IActionResult AddCity([FromForm] City city ,IFormFile file )
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var folderPath = @"..\MVC\wwwroot\images";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var imageUrl = Path.Combine("/images", file.FileName);
            city.c_city_photo = imageUrl;
            HttpContext.Session.SetInt32("userid", city.c_userid);

            if (_cityRepo.AddCity(city))
            {
                return Ok();
            }
            return BadRequest(new { success = false, message = "Failed to add city" });
        }

        [HttpPut]
        public IActionResult UpdateCity(int id, [FromForm] City? city = null, IFormFile? file = null)
        {
            var existingCity = _cityRepo.FetchByCityId(id);
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
                var folderPath = @"..\MVC\wwwroot\images";

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

            if (_cityRepo.UpdateCity(city))
            {
                return Ok();
            }

            return BadRequest(new { success = false, message = "Failed to update city" });
        }


        [HttpDelete]
        public IActionResult DeleteCity(int id)
        {
            _cityRepo.deleteCity(id);

            return Ok();

        }

        [HttpPost("SaveImage")]
        public IActionResult SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var folderPath = @"C:\isha\casepoint\WEBAPP\webappdemo\WEBAPP\wwwroot\images";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var imageUrl = Path.Combine("/images", file.FileName);
            return Ok(new { imageUrl });
        }


    }
}