namespace BankTest.API.Models.Dto
{
    public class UserDto
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string EMail { get; set; }
        public string? Address { get; set; }
    }
}
