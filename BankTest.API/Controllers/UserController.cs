using BankTest.API.Models.Dto;
using BankTest.API.Models;
using BankTest.API.Models.Helpers;
using BankTest.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankTest.Library.Models.Dto;

namespace BankTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BankTestDBService _context;
        private readonly ILogger<UserController> _logger;
        private readonly TokenService _tokenService;
        public UserController(
            BankTestDBService context,
            ILogger<UserController> logger,
            TokenService tokenService
            )
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Authorize]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if(!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Create the new User
                var newUser = new User
                {
                    Login = createUserDto.Login,
                    Password = createUserDto.Password,
                    EMail = createUserDto.EMail,
                    Name = createUserDto.Name,
                    Address = createUserDto.Address
                };

                // Add and save to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserController/CreateUser EMail: {(string.IsNullOrEmpty(createUserDto.EMail) ? "Unknown" : createUserDto.EMail)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(a => a.Id == id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserController/GetUserById Id: {id}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Find the user in the database
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                // Remove the user
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UserController/DeleteUser Id: {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            List<string> UsersNames = _context.Users.Select(uName => uName.Name).ToList();
            return Ok(UsersNames);
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserDto")]
        public async Task<IActionResult> GetUserDto()
        {
            List<UserInfo> outAll = new List<UserInfo>();
            outAll = _context.Users.Select(x => new UserInfo
            {
                EMail = x.EMail,
                Login = x.Login,
                Name = x.Name,
                Password = x.Password
            }).ToList();
            return Ok(outAll);
        }
    }
}
