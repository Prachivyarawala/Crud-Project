using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories;
// using Npgsql


namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepositories _userrepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserRepositories userrepo, IHttpContextAccessor httpContextAccessor)
        {
            _userrepo = userrepo;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
           
        }

        [Route("")]
        public IActionResult Index()
        {

            ViewBag.IsAuthenticated = false;

            return View();
        }



        public IActionResult Register()
        {
            ViewBag.IsAuthenticated = false;
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                if (!_userrepo.Register(user))
                {
                    ModelState.AddModelError("EmailExists", "Email already exists. Please choose a different email.");
                    ViewBag.IsAuthenticated = false;
                    return View(user);
                }

                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }

            ViewBag.IsAuthenticated = false;
            return View(user);
        }



        public IActionResult Login()
        {
            ViewBag.IsAuthenticated = false;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (string.IsNullOrEmpty(user.c_useremail) || string.IsNullOrEmpty(user.c_userpassword))
            {
                ViewBag.IsAuthenticated = false;
                return View(user);
            }

            User loginUser = _userrepo.Login(user.c_useremail, user.c_userpassword);
            if (loginUser != null)
            {
                var session = _httpContextAccessor.HttpContext.Session;

                session.SetInt32("IsAuthenticated", 1); 
                ViewBag.IsAuthenticated = true;
                if (user.c_useremail.Equals("admin@gmail.com") && user.c_userpassword.Equals("123456"))
                {
                    return RedirectToAction("Index", "City");
                }
                return RedirectToAction("Index", "City");
            }
            else
            {
                ViewBag.IsAuthenticated = false;
                ViewData["ErrorMessage"] = "Wrong email or password. Please try again.";
                return View(user);
            }
        }


        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear(); 
            ViewBag.IsAuthenticated = false;
            return RedirectToAction("Index", "User");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
