using inventory_system_api.Application.IService;
using inventory_system_api.Controllers;
using inventory_system_api.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockRepo;
        private readonly ProductController _controller;
        private CancellationToken cancellationToken = CancellationToken.None;

        public ProductControllerTests()
        {
            _mockRepo = new Mock<IProductService>();
            _controller = new ProductController(_mockRepo.Object);
            _controller.SetMockUser(userId: 1);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var fakeProduct = new Product
            {
                ID = productId,
                EngName = "Test Product",
                NepName = "परीक्षण उत्पादन",
                GroupID = 2,
                Code = "P001",
                Remarks = "Sample product for unit test",
                UnitID = 3,
                UnitName = "Piece",
                UnitSymbol = "pc",
                SalesRate = 150.50m,
                OpeningQuantity = 10.0m,
                PurchaseRate = 120.75m,
                PurchaseDiscount = 5.0m,
                IsBuiltIn = false,
                IsActive = true,
                CreatedBy = "unittest",
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = "unittest",
                ModifiedDate = DateTime.UtcNow,
                BackColor = "#FFFFFF",
                IsVatApplicable = true,
                IsInventoryApplicable = true,
                DebtorsID = null,
                IsDecimalApplicable = false,
                ContactPerson = "John Doe",
                Address1 = "123 Test St",
                Address2 = "Suite 1",
                City = "Testville",
                Telephone = "1234567890",
                Email = "test@example.com",
                Company = "TestCo",
                Website = "https://example.com",
                CompanyID = 1,
                ParentProductID = null,
                Size = "M",
                TaxID = 5,
            };

            // use mock to make a fake Product object for test
            _mockRepo.Setup(repo => repo.Get(productId, cancellationToken)).ReturnsAsync(fakeProduct);

            // Act
            var result = await _controller.Get(productId, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedProduct = Assert.IsType<Product>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("Test Product", returnedProduct.EngName);
        }

        [Fact]
        public async Task AddEdit_ReturnsOk_OnSuccess()
        {
            // Arrange
            var newProduct = new Product
            {
                ID = 0,
                EngName = "New Product",
                NepName = "नयाँ उत्पादन",
                GroupID = 2,
                Code = "P002",
                Remarks = "New product for add/edit test",
                UnitID = 3,
                UnitName = "Piece",
                UnitSymbol = "pc",
                SalesRate = 200.00m,
                OpeningQuantity = 5.0m,
                PurchaseRate = 180.00m,
                PurchaseDiscount = 0.0m,
                IsBuiltIn = false,
                IsActive = true,
                CreatedBy = "unittest",
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = null,
                ModifiedDate = null,
                BackColor = "#000000",
                IsVatApplicable = false,
                IsInventoryApplicable = true,
                DebtorsID = null,
                IsDecimalApplicable = true,
                ContactPerson = "Jane Doe",
                Address1 = "456 Test Ave",
                Address2 = null,
                City = "Example City",
                Telephone = "0987654321",
                Email = "new@example.com",
                Company = "NewCo",
                Website = "https://new.example.com",
                CompanyID = 1,
                ParentProductID = null,
                Size = "L",
                TaxID = 3,
            };
            _mockRepo.Setup(repo => repo.AddEdit(It.IsAny<Product>(), cancellationToken,1)).ReturnsAsync(1);

            // Act
            var result = await _controller.Post(newProduct, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode); // this is the HTTP response
            Assert.Equal(200, returned.StatusCode); // this is our custom response code
            Assert.Equal("Success", returned.Message);
            Assert.NotNull(returned.Data); // just checking if the data is null or not for now.. but later i have to check the id as well 
            _mockRepo.Verify(repo => repo.AddEdit(It.IsAny<Product>(), cancellationToken,1), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenProductIsDeleted()
        {
            // Arrange
            int productIdToDelete = 10;

            _mockRepo.Setup(repo => repo.Delete(productIdToDelete, cancellationToken,1))
                     .ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(productIdToDelete, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Success", response.Message);
            Assert.Null(response.Data);

            _mockRepo.Verify(repo => repo.Delete(productIdToDelete, cancellationToken,1), Times.Once);
        }
    }
}