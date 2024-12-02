using BankTest.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankTest.API.Services
{
    public class BankTestDBService : DbContext
    {

        public BankTestDBService() { }

        public BankTestDBService(DbContextOptions<BankTestDBService> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
