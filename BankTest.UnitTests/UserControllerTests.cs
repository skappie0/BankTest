using BankTest.API.Controllers;
using BankTest.API.Models;
using BankTest.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using BankTest.API.Models.Dto;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BankTest.UnitTests
{
    public class UserControllerTests
    {
        private BankTestDBService _bankTestDBService;
        private Mock<ILogger<UserController>> _mockLogger;
        private Mock<TokenService> _mockTokenService;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<UserController>>();
            _configuration = new ConfigurationBuilder().Build();
            _mockTokenService = new Mock<TokenService>(_configuration);

            var options = new DbContextOptionsBuilder<BankTestDBService>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bankTestDBService = new BankTestDBService(options);
        }

        [Test]
        public void CreateUser_ReturnOk()
        {
            //Arrange
            _bankTestDBService.Users.Add(new User
            {
                Id = 1,
                Name = "TestUser",
                EMail = "test@example.com",
                Password = "password123",
                Login = "test"
            });
            _bankTestDBService.SaveChanges();

            var createUserDto = new UserDto
            {
                Login = "testuser",
                Password = "password123",
                EMail = "test@example.com",
                Name = "Test User",
                Address = "123 Test Street"
            };
            
            _mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<IdentityUser>(), 5)).Returns("someToken");

            var httpContext = new DefaultHttpContext();
        
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser"), new Claim(ClaimTypes.Email, "test@example.com") }));


            var controller = new UserController(_bankTestDBService, _mockLogger.Object, _mockTokenService.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            //Act
            var result = controller.CreateUser(createUserDto);

            //Assert
            NUnit.Framework.Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            NUnit.Framework.Assert.That(result.Result, Is.Not.Null);
        }
        [Test]
        public void CreateUser_ReturnForbidden()
        {
            //Arrange
            _bankTestDBService.Users.Add(new User
            {
                Id = 2,
                Name = "TestUser",
                EMail = "test@example.com",
                Password = "password123",
                Login = "test"
            });
            _bankTestDBService.SaveChanges();

            var createUserDto = new UserDto
            {
                Login = "testuser",
                Password = "password123",
                EMail = "test@example.com",
                Name = "Test User",
                Address = "123 Test Street"
            };

            _mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<IdentityUser>(), 5)).Returns("someToken");

            var httpContext = new DefaultHttpContext();

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser"), new Claim(ClaimTypes.Email, "test@example.com") }));

            var controller = new UserController(_bankTestDBService, _mockLogger.Object, _mockTokenService.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            //Act
            var result = controller.CreateUser(createUserDto);

            //Assert
            NUnit.Framework.Assert.That(result.Result, Is.TypeOf<ForbidResult>());
        }
        [TearDown]
        public void TearDown()
        {
            _bankTestDBService?.Dispose();
        }
    }
}
