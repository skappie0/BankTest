namespace BankTest.API.Models
{
    public class AccountRequest
    {
        public required string EMail { get; set; }
        public required string AccountNumber { get; set; }
        public string? Description { get; set; }
    }
}
