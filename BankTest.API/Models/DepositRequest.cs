﻿using System.ComponentModel.DataAnnotations;

namespace BankTest.API.Models
{
    public class DepositRequest
    {
        public required string EMail { get; set; }
        public string DepositAccount { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public required string Text { get; set; }
    }
}