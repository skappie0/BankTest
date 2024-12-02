namespace BankTest.API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required int UserId { get; set; }

        public User User { get; set; } = null!;

        public required string AccountNumber { get; set; }
        public string? Description { get; set; }
    }
}
