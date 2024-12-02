namespace BankTest.API.Models.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required string AccountNumber { get; set; }
        public string? Description { get; set; }
        public decimal Balance { get; set; }
    }
}
