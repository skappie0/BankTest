using System.ComponentModel.DataAnnotations;

namespace BankTest.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public required int AccountDebitId { get; set; }
        public Account AccountDebit { get; set; } = null!;

        public required int AccountCreditId { get; set; }
        public Account AccountCredit { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public required string Text { get; set; }
    }
}
