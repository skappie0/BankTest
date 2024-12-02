using BankTest.API.Models.Dto;
using BankTest.API.Models;
using BankTest.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankTest.API.Models.Helpers;

namespace BankTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly BankTestDBService _context;
        private readonly ILogger<TransactionController> _logger;
        private readonly TokenService _tokenService;
        public TransactionController(
            BankTestDBService context,
            ILogger<TransactionController> logger,
            TokenService tokenService//,
            )
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Authorize]
        [Route("CreateDeposit")]
        public async Task<IActionResult> CreateDeposit([FromBody] DepositRequest depositRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Check if the user exists
                var user = _context.Users.First(p => p.EMail == depositRequest.EMail);
                if (user == null)
                    return NotFound($"User with Email {depositRequest.EMail} not found.");

                var accountDebit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == "11111111");
                if (accountDebit == null)
                    return NotFound($"Account with AccountNumber 11111111 not found.");

                var accountCredit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == depositRequest.DepositAccount);
                if (accountCredit == null)
                    return NotFound($"Account with AccountNumber {depositRequest.DepositAccount} not found.");

                // Create the new transaction
                var transaction = new Transaction
                {
                    AccountDebitId = accountDebit.Id,
                    AccountCreditId = accountCredit.Id,
                    Amount = depositRequest.Amount,
                    Text = depositRequest.Text
                };

                // Add and save to the database
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionController/CreateDeposit AccountDebit: 11111111, " +
                    $"AccountCredit: {depositRequest.DepositAccount}, Amount: {depositRequest.Amount}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("CreateWithrdaw")]
        public async Task<IActionResult> CreateWithrdaw([FromBody] WithdrawRequest withdrawRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Check if the user exists
                var user = _context.Users.First(p => p.EMail == withdrawRequest.EMail);
                if (user == null)
                    return NotFound($"User with Email {withdrawRequest.EMail} not found.");

                var accountDebit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == withdrawRequest.WithdrawAccount);
                if (accountDebit == null)
                    return NotFound($"Account with AccountNumber {withdrawRequest.WithdrawAccount} not found.");

                var accountCredit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == "11111111");
                if (accountCredit == null)
                    return NotFound($"Account with AccountNumber 11111111 not found.");


                // Create the new transaction
                var transaction = new Transaction
                {
                    AccountDebitId = accountDebit.Id,
                    AccountCreditId = accountCredit.Id,
                    Amount = withdrawRequest.Amount,
                    Text = withdrawRequest.Text
                };

                // Add and save to the database
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionController/CreateWithrdaw AccountCredit: 11111111, " +
                    $"AccountDebit: {withdrawRequest.WithdrawAccount}, Amount: {withdrawRequest.Amount}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("CreateTransfer")]
        public async Task<IActionResult> CreateTransfer([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? "";

                // Check if the user exists
                var user = _context.Users.First(p => p.EMail == pEmail);
                if (user == null)
                    return NotFound($"User with Email {pEmail} not found.");

                // Check if the user account exists
                if (user.Id != 1)
                {
                    var userAccount = _context.Accounts.First(p => p.AccountNumber == transactionDto.AccountDebit && p.UserId == user.Id);
                    if (userAccount == null)
                        return NotFound($"Account {transactionDto.AccountDebit} for user {user.Name} not found.");
                }

                var accountDebit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == transactionDto.AccountDebit);
                if (accountDebit == null)
                    return NotFound($"Account with AccountNumber {transactionDto.AccountDebit} not found.");

                var accountCredit = await _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AccountNumber == transactionDto.AccountCredit);
                if (accountCredit == null)
                    return NotFound($"Account with AccountNumber {transactionDto.AccountCredit} not found.");

                // Create the new transaction
                var transaction = new Transaction
                {
                    AccountDebitId = accountDebit.Id,
                    AccountCreditId = accountCredit.Id,
                    Amount = transactionDto.Amount,
                    Text = transactionDto.Text
                };

                // Add and save to the database
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionController/CreateTransfer AccountDebit: {transactionDto.AccountDebit}, " +
                    $"AccountCredit: {transactionDto.AccountCredit}, Amount: {transactionDto.Amount}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetTransactionById")]
        public IActionResult GetTransactionById(int id)
        {
            try
            {
                var transaction = _context.Transactions
                    .FirstOrDefault(a => a.Id == id);

                if (transaction == null)
                    return NotFound();

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionController/GetTransactionById Id: {id}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        [Route("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                if (!CommonFunctions.BankUserCheck(HttpContext, _context))
                    return Forbid();

                // Find the user in the database
                var user = await _context.Transactions.FindAsync(id);

                if (user == null)
                {
                    return NotFound($"Transaction with ID {id} not found.");
                }

                // Remove the user
                _context.Transactions.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TransactionController/DeleteTransaction Id: {id}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
