using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class TaxControllerTests
    {
        private readonly Mock<ITaxRepository> _mockRepo;
        private readonly TaxController _controller;
        private CancellationToken cancellationToken = CancellationToken.None;

        public TaxControllerTests()
        {
            _mockRepo = new Mock<ITaxRepository>();
            _controller = new TaxController(_mockRepo.Object);
            _controller.SetMockUser(userId: 1);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenTaxExists()
        {
            // Arrange
            var taxId = 1;
            var fakeTax = new Tax { ID = taxId, Name = "VAT", Rate = 13 };

            // use mock to make a fake Tax object for test
            _mockRepo.Setup(repo => repo.Get(taxId, cancellationToken)).ReturnsAsync(fakeTax);

            // Act
            var result = await _controller.Get(taxId, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedTax = Assert.IsType<Tax>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("VAT", returnedTax.Name);
        }

        [Fact]
        public async Task AddEdit_ReturnsOk_OnSuccess()
        {
            // Arrange
            var newTax = new Tax { Name = "Service Tax", Rate = 5 };
            _mockRepo.Setup(repo => repo.AddEdit(It.IsAny<Tax>(), 1, cancellationToken)).ReturnsAsync(1);

            // Act
            var result = await _controller.Post(newTax, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTax = Assert.IsType<Models.Response>(okResult.Value);

            var expectedData = new { ID = 1 };  
            // Assert
            Assert.Equal(200, okResult.StatusCode); // this is the main response
            Assert.Equal(200, returnedTax.StatusCode); // this is out custom code
            Assert.Equal("Success", returnedTax.Message);
            Assert.NotNull(returnedTax.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenTaxIsDeleted()
        {
            // Arrange
            int taxIdToDelete = 10;

            _mockRepo.Setup(repo => repo.Delete(taxIdToDelete, 1, cancellationToken))
                     .ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(taxIdToDelete, cancellationToken);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<Models.Response>(okResult.Value);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Success", response.Message);
            //Assert.Equal(1, response.Data);

            _mockRepo.Verify(repo => repo.Delete(taxIdToDelete, 1, cancellationToken), Times.Once);
        }
    }
}