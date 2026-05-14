using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserRepository> _mockRepo;
        private CancellationToken cancellationToken = CancellationToken.None;

        // xUnit uses the Constructor instead of [TestInitialize]
        public UserControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "K9x7Y2pLqS8zV6rTnW3mE4bUoF1dH5jX"},
                {"Jwt:Issuer", "inventory-system-api"}
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockRepo = new Mock<IUserRepository>();
            _controller = new UserController(config, _mockRepo.Object);
        }

        [Fact]
        public async Task Login_Returns_Token_On_Valid_User()
        {
            // Arrange
            var loginUser = new LoginUser
            {
                UserName = "san",
                Password = "san1",
                Email = "san@test.com"
            };

            var user = new User
            {
                ID = 1,
                UserName = "san",
                Name = "San",
                Email = "san@test.com",
                Password = "san1",
                Role = "Admin",
                Address = string.Empty,
                PhoneNo = string.Empty
            };

            _mockRepo.Setup(r => r.VerifyAndGetUserDetails(loginUser.UserName, loginUser.Password, cancellationToken)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginUser, cancellationToken);

            // Assert - check the return type and payload
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.NotNull(resp.Data);
        }
    }
}

