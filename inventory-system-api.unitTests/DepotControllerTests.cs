//using accswift_api.Controllers;
//using inventory_system_api.Application.IRepository.Invenetory;
//using inventory_system_api.Application.Models.Inventory;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Assert = Xunit.Assert;

//namespace inventory_system_api.unitTests
//{
//    public class DepotControllerTests
//    {
//        private readonly Mock<IDepotRepository> _mockRepo;
//        private readonly DepotController _controller;

//        public DepotControllerTests()
//        {
//            _mockRepo = new Mock<IDepotRepository>();
//            _controller = new DepotController(_mockRepo.Object);
//        }

//        [Fact]
//        public async Task Get_ReturnsOk_WithList()
//        {
//            // Arrange
//            var list = new List<Depot>
//            {
//                new Depot { ID = 1, Name = "Main Depot" },
//                new Depot { ID = 2, Name = "Secondary" }
//            };
//            _mockRepo.Setup(r => r.Get()).ReturnsAsync(list);

//            // Act
//            var result = await _controller.Get();

//            // Assert
//            var ok = Assert.IsType<OkObjectResult>(result);
//            var resp = Assert.IsType<Models.Response>(ok.Value);
//            var data = Assert.IsType<List<Depot>>(resp.Data);

//            Assert.Equal(200, ok.StatusCode);
//            Assert.Equal(200, resp.StatusCode);
//            Assert.Equal("Success", resp.Message);
//            Assert.Equal(2, data.Count);
//            Assert.Equal("Main Depot", data[0].Name);

//            _mockRepo.Verify(r => r.Get(), Times.Once);
//        }

//        [Fact]
//        public async Task GetById_ReturnsOk_WhenExists()
//        {
//            // Arrange
//            var id = 3;
//            var item = new Depot { ID = id, Name = "Depot X" };
//            _mockRepo.Setup(r => r.Get(id)).ReturnsAsync(item);

//            // Act
//            var result = await _controller.Get(id);

//            // Assert
//            var ok = Assert.IsType<OkObjectResult>(result);
//            var resp = Assert.IsType<Models.Response>(ok.Value);
//            var data = Assert.IsType<Depot>(resp.Data);

//            Assert.Equal(200, resp.StatusCode);
//            Assert.Equal("Depot X", data.Name);
//            _mockRepo.Verify(r => r.Get(id), Times.Once);
//        }

//        [Fact]
//        public async Task Post_ReturnsOk_OnSuccess()
//        {
//            var model = new Depot { ID = 0, Name = "New Depot" };
//            _mockRepo.Setup(r => r.AddEdit(It.IsAny<Depot>())).ReturnsAsync(1);

//            var result = await _controller.Get(model); // DepotController has two Post/Get overloads; using Get(Post) signature? Use Post method is overloaded - controller defines Get(Depot entity) as POST - actually controller shows [HttpPost] public async Task<IActionResult> Get(Depot entity) { ... }

//            var ok = Assert.IsType<OkObjectResult>(result);
//            var resp = Assert.IsType<Models.Response>(ok.Value);

//            Assert.Equal(200, ok.StatusCode);
//            Assert.Equal(200, resp.StatusCode);
//            Assert.Equal("Success", resp.Message);
//            _mockRepo.Verify(r => r.AddEdit(It.IsAny<Depot>()), Times.Once);
//        }

//        [Fact]
//        public async Task Delete_ReturnsOk_WhenDeleted()
//        {
//            _mockRepo.Setup(r => r.Delete(4, cancellationToken)).ReturnsAsync(1);

//            var result = await _controller.Delete(4);

//            var ok = Assert.IsType<OkObjectResult>(result);
//            var resp = Assert.IsType<Models.Response>(ok.Value);

//            Assert.Equal(200, resp.StatusCode);
//            Assert.Equal("Success", resp.Message);
//            _mockRepo.Verify(r => r.Delete(4), Times.Once);
//        }
//    }
//}
