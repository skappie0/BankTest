using BankTest.API.Models;
using BankTest.API.Models.Dto;
using BankTest.API.Models.Helpers;
using BankTest.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankTest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly BankTestDBService _context;
        private readonly ILogger<AccountController> _logger;
        private readonly TokenService _tokenService;
        public AccountController(
            BankTestDBService context,
            ILogger<AccountController> logger,
            TokenService tokenService//,
            )
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Authorize]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequest accountRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Check if the user exists
                var user = _context.Users.First(p => p.EMail == accountRequest.EMail);
                if (user == null)
                    return NotFound($"User with Email {accountRequest.EMail} not found.");

                // Create the new Account
                var account = new Account
                {
                    UserId = user.Id,
                    AccountNumber = accountRequest.AccountNumber,
                    Description = accountRequest.Description
                };

                // Add and save to the database
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AccountController/CreateAccount AccountNumber: " +
                    $"{(string.IsNullOrEmpty(accountRequest.AccountNumber) ? "Unknown" : accountRequest.AccountNumber)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetAccountById")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var account = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                    return NotFound();

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AccountController/GetAccountById AccountNumber: {id}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Find the account in the database
                var account = await _context.Accounts.FindAsync(id);

                if (account == null)
                {
                    return NotFound($"Account with ID {id} not found.");
                }

                // Remove the account
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AccountController/DeleteAccount Id: {id}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var query = string.Empty;
            if (CommonFunctions.BankUserCheck(HttpContext, _context))
            {
                query = @"
                    SELECT a.Id, a.UserId, a.AccountNumber, a.Description, dbo.getBalance(a.Id) Balance
                    FROM Accounts a
                    INNER JOIN Users u ON a.UserId = u.Id";
            }
            else
            {
                var pEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? "";
                // Check if the user exists
                var user = _context.Users.First(p => p.EMail == pEmail);
                if (user == null)
                    return NotFound($"User with Email {pEmail} not found.");

                query = @$"
                    SELECT a.Id, a.UserId, a.AccountNumber, a.Description, dbo.getBalance(a.Id) Balance
                    FROM Accounts a
                    INNER JOIN Users u ON a.UserId = u.Id
                    where a.UserId = {user.Id}";
            }

            var results = await _context.Database
                    .SqlQueryRaw<AccountDto>(query)
                    .ToListAsync();

            return Ok(results);
        }
    }
}
