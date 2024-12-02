namespace BankTest.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string EMail { get; set; }
        public string? Address { get; set; }
    }
}
