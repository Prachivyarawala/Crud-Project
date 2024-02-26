using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;




namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserRepositories _userrepo;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public UserApiController(IUserRepositories userrepo, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userrepo = userrepo;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                User loginUser = _userrepo.Login(request.Email, request.Password);
                if (loginUser != null)
                {
                    HttpContext.Session.SetInt32("userid", loginUser.c_userid.GetValueOrDefault());
                    HttpContext.Session.SetString("username", loginUser.c_username);

                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("userid", loginUser.c_userid.ToString()),
                new Claim("username", loginUser.c_username),
                new Claim("email", loginUser.c_useremail)
            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signIn
                    );

                    return Ok(new { userid = loginUser.c_userid, username = loginUser.c_username });
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] User User)
        {
            bool ans = _userrepo.Register(User);
            return Ok(ans);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userrepo.GetOne(id);
            if (user == null)
            {
                return NotFound(); // User not found
            }
            return Ok(user);
        }

        [HttpPost("Update/{id}")]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateUser(int id, [FromForm] User user)
        {
            var existingUser = _userrepo.GetOne(id);
            if (existingUser == null)
            {
                return NotFound(); // User not found
            }

            // Update user details with the form data
            existingUser.c_username = user.c_username;
            existingUser.c_useremail = user.c_useremail;
            existingUser.c_userpassword = user.c_userpassword;

            // Update other properties as needed

            // Save the updated user details
            _userrepo.Update(existingUser);

            return Ok(existingUser);
        }

        // [Authorize]
        // [HttpGet]
        // [Route("SecureEndpoint")]
        // public IActionResult SecureEndpoint()
        // {
        //     // Authorized endpoint logic goes here
        //     var userIdClaim = User.FindFirst("Userid"); // Corrected claim name
        //     if (userIdClaim != null)
        //     {
        //         var userId = userIdClaim.Value;
        //         return Ok($"This is a secure endpoint. UserId: {userId}");
        //     }
        //     else
        //     {
        //         return BadRequest("UserId claim not found in token");
        //     }
        // }



    }



}
