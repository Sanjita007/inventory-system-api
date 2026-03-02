using inventory_system_api.Application.Models;
using inventory_system_api.Controllers.User;
using inventory_system_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Inventory_system_api.unitTests
{
    public class LoginControllerTests
    {
        private readonly LoginController _controller;

        // xUnit uses the Constructor instead of [TestInitialize]
        public LoginControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "K9x7Y2pLqS8zV6rTnW3mE4bUoF1dH5jX"},
                {"Jwt:Issuer", "inventory-system-api"}
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new LoginController(config);
        }

        [Fact] // Replaces [TestMethod]
        public void Login_Returns_Token_On_Valid_User()
        {
            // Arrange
            var loginUser = new LoginUser
            {
                UserName = "san",
                Password = "san",
                Email = "san@test.com"
            };

            // Act
            var result = _controller.Login(loginUser);

            // check the return type
            var okResult = Assert.IsInstanceOfType<OkObjectResult>(result);

            // Assert
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}

