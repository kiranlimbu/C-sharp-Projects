using System;
using System.Threading.Tasks;
using bugspotAPI.Dtos;
using bugspotAPI.Helpers;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository userRepo, JwtService jwtService)
        {
            _repository = userRepo;
            _jwtService = jwtService;
        }

        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            UserModel newUser = new UserModel {
                fName = user.fName,
                lName = user.lName,
                Email = user.Email,
                UserName = user.Email,
                password = BCrypt.Net.BCrypt.HashPassword(user.password),
                confirmPassword = BCrypt.Net.BCrypt.HashPassword(user.confirmPassword)
            };

            return Created("Success!", await _repository.Create(newUser));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            try
            {
                UserModel existingUser = await _repository.GetByEmailAsync(user.UserName);

                if (existingUser == null)
                    return BadRequest(new {message = "Invalid User or Password."});
                
                bool isCorrect = BCrypt.Net.BCrypt.Verify(user.password, existingUser.password);

                if (!isCorrect)
                    return BadRequest(new {message = "Invalid User or Password."});
                // Generate JWT token
                string jwt = _jwtService.Generate(existingUser.Id);

                Response.Cookies.Append("jwt", jwt, new CookieOptions {
                    HttpOnly = true
                });

                return Ok("Successful!");
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("User")]
        public IActionResult GetUser()
        {
            try
            {
                // get cookies from user browser
                var jwt = Request.Cookies["jwt"];
                // validate cookies
                var token = _jwtService.Verify(jwt);
                // get userId 
                string userId = token.Issuer;
                // get user information
                var user = _repository.GetById(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok( new
                {
                    message = "You have successfully logged out."
                }
            );
        }
    }
}