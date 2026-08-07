using accswift_api.Controllers;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class UnitControllerTests
    {
        private readonly Mock<IUnitRepository> _mockRepo;
        private readonly UnitController _controller;
        private CancellationToken cancellationToken = CancellationToken.None;

        public UnitControllerTests()
        {
            _mockRepo = new Mock<IUnitRepository>();
            _controller = new UnitController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenTaxExists()
        {
            // Arrange
            var unitId = 1;
            var fakeUnit = new Unit { ID = unitId, Name = "Ton", Symbol="T"};

            // use mock to make a fake Tax object for test
            _mockRepo.Setup(repo => repo.Get(unitId, cancellationToken)).ReturnsAsync(fakeUnit);

            // Act
            var result = await _controller.Get(unitId, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedTax = Assert.IsType<Unit>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("Ton", returnedTax.Name);
        }

        [Fact]
        public async Task AddEdit_ReturnsOk_OnSuccess()
        {
            // Arrange
            var newUnit = new Unit { ID = 1, Name = "Ton", Symbol = "T" };
            _mockRepo.Setup(repo => repo.AddEdit(It.IsAny<Unit>(), cancellationToken, 1)).ReturnsAsync(1);

            // Act
            var result = await _controller.Post(newUnit, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTax = Assert.IsType<Models.Response>(okResult.Value);

            // Assert
            Assert.Equal(200, okResult.StatusCode); // this is the main response
            Assert.Equal(200, returnedTax.StatusCode); // this is out custom code
            Assert.Equal("Success", returnedTax.Message);
            //Assert.Equal(1, returnedTax.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenTaxIsDeleted()
        {
            // Arrange
            int taxIdToDelete = 10;

            _mockRepo.Setup(repo => repo.Delete(taxIdToDelete, cancellationToken, 1))
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

            _mockRepo.Verify(repo => repo.Delete(taxIdToDelete, cancellationToken, 1), Times.Once);
        }
    }
}