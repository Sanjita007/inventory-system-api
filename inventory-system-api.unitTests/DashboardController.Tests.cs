using accswift_api.Controllers;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class DashboardControllerTests
    {
        private readonly Mock<IDashboardSummaryRepository> _mockRepo;
        private readonly DashboardController _controller;

        public DashboardControllerTests()
        {
            _mockRepo = new Mock<IDashboardSummaryRepository>();
            _controller = new DashboardController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetProduct_ReturnsOk_WhenProductsExists()
        {
            // Arrange
            var fakeProducts = new List<ProductSummary> { new()
            {
                ProductName = "Product A",
                SalesPrice = 100.00m,
                Image = "image_url_B"
            },
            new()
            {
                ProductName = "Product B",
                SalesPrice = 30.8m,
                Image = "image_url_B"
            }
            };

            // use mock to make a fake Tax object for test
            _mockRepo.Setup(repo => repo.GetProductDashboardSummary()).ReturnsAsync(fakeProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedProducts = Assert.IsType<List<ProductSummary>>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("Product A", returnedProducts[0].ProductName);
        }

        [Fact]
        public async Task GetPurchSalesReturnsOk_WhenPurchSalesExists()
        {
            // Arrange
            var fakeProducts = new List<ProductSummary> { new()
            {
                ProductName = "Product A",
                SalesPrice = 100.00m,
                Image = "image_url_B"
            },
            new()
            {
                ProductName = "Product B",
                SalesPrice = 30.8m,
                Image = "image_url_B"
            }
            };

            // use mock to make a fake Tax object for test
            _mockRepo.Setup(repo => repo.GetProductDashboardSummary()).ReturnsAsync(fakeProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedProducts = Assert.IsType<List<ProductSummary>>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("Product A", returnedProducts[0].ProductName);
        }

    }
}