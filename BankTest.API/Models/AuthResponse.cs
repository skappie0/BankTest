﻿namespace BankTest.API.Models
{
    public class AuthResponse
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
