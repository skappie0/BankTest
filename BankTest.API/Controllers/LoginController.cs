using BankTest.API.Models;
using BankTest.API.Services;
using BankTest.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankTest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly BankTestDBService _context;
        private readonly ILogger<LoginController> _logger;
        private readonly TokenService _tokenService;
        private readonly int _expirationMinutes = 5;

        public LoginController(
            BankTestDBService context,
            ILogger<LoginController> logger,
            TokenService tokenService
            )
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpGet]
        [Authorize]
        [Route("AuthorizeCheck")]
        public ActionResult<int> AuthorizeCheck()
        {
            return Ok(12345);
        }

        [HttpPost]
        [Route("UserLogin")]
        public ActionResult<AuthResponse> UserLogin(AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Users.Any(p => p.EMail == request.Email && p.Password == request.Password))
            {
                var user = _context.Users.First(p => p.EMail == request.Email && p.Password == request.Password);

                var userIdn = new IdentityUser() { Email = user.EMail, UserName = user.Name };

                var accessToken = _tokenService.CreateToken(userIdn, _expirationMinutes);

                return Ok(new AuthResponse
                {
                    Username = user.Name,
                    Email = user.EMail,
                    Token = accessToken,
                    Expiration = DateTime.Now.AddMinutes(_expirationMinutes)
                });
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
