using System.ComponentModel.DataAnnotations;

namespace BankTest.API.Models.Dto
{
    public class TransactionDto
    {
        public string AccountDebit { get; set; } = null!;

        public string AccountCredit { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public required string Text { get; set; }
    }
}
