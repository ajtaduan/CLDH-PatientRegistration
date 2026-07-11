using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CLDH.PatientRegistration.Data;
using CLDH.PatientRegistration.DTOs;
using CLDH.PatientRegistration.Services;

namespace CLDH.PatientRegistration.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AuthService _authService;

        // ASP.NET injects these automatically, I don't create them manually
        public AuthController(AppDbContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Find the user by username
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            // Same error message wether username or password is wrong
            // Don't want to tell an attacker which one they got wrong
            if (user == null || !_authService.VerifyPassword(user, user.PasswordHash, request.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Claims = pieces of info stored in the login cookie about who's logged in
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // This actually sets the login cookie on the response
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new { message = "Login Successful", username = user.Username });
        }

        [HttpPost("logout")]
        [Authorize] // must be logged in to log out
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("me")]
        [Authorize] // lets frontend check "am I still logged in?" on page load
        public IActionResult Me()
        {
            return Ok(new { username = User.Identity?.Name });
        }
    }
}


