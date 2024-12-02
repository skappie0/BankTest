namespace BankTest.API.Models
{
    public class Client
    {

        public int Id { get; set; }
        public required string Name { get; set; }

        public string? Address { get; set; }
    }
}
