using BankTest.API.Controllers;
using BankTest.API.Models;
using BankTest.API.Services;
using BankTest.Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BankTest.UnitTests
{
    public class LoginControllerTests
    {
        private BankTestDBService _bankTestDBService;
        private Mock<ILogger<LoginController>> _mockLogger;
        private Mock<TokenService> _mockTokenService;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<LoginController>>();
            _configuration = new ConfigurationBuilder().Build();
            _mockTokenService = new Mock<TokenService>(_configuration);

            var options = new DbContextOptionsBuilder<BankTestDBService>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bankTestDBService = new BankTestDBService(options);
            _bankTestDBService.Users.Add(new User
            {
                Id = 1,
                Name = "TestUser",
                EMail = "test@example.com",
                Password = "password123",
                Login = "test"
            });
            _bankTestDBService.SaveChanges();
        }

        [Test]
        public void UserLogin_ReturnOkWithToken()
        {
            //Arrange
            AuthRequest request = new AuthRequest { Email = "test@example.com", Password = "password123" };
            _mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<IdentityUser>(), 5)).Returns("someToken");
            var controller = new LoginController(_bankTestDBService, _mockLogger.Object, _mockTokenService.Object);

            //Act
            var result = controller.UserLogin(request);

            //Assert
            NUnit.Framework.Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            _mockTokenService.Verify(ts => ts.CreateToken(It.IsAny<IdentityUser>(), 5), Times.Once);
        }
        [Test]
        public void UserLogin_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AuthRequest
            {
                Email = "wrong@example.com",
                Password = "wrongpassword"
            };

            _mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<IdentityUser>(), 5)).Returns("someToken");
            var controller = new LoginController(_bankTestDBService, _mockLogger.Object, _mockTokenService.Object);

            // Act
            var result = controller.UserLogin(request);

            // Assert
            NUnit.Framework.Assert.That(result, Is.Not.Null);
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<UnauthorizedResult>());
        }
        /*[TearDown]
        public void TearDown()
        {
            _bankTestDBService?.Dispose();
        }*/
    }
}
