using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class MvcUserController : Controller
    {
        private readonly ILogger<MvcUserController> _logger;

        public MvcUserController(ILogger<MvcUserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login(){
            return View();
        }


        [Route("Register")]
        public IActionResult Register(){
            return View();
        }

        
        [Route("logout")]
        public IActionResult Logout(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}