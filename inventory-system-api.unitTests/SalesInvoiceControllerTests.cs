using accswift_api.Controllers;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class SalesInvoiceControllerTests
    {
        private readonly Mock<ISalesInvoiceService> _mockRepo;
        private readonly SalesInvoiceController _controller;

        public SalesInvoiceControllerTests()
        {
            _mockRepo = new Mock<ISalesInvoiceService>();
            _controller = new SalesInvoiceController(_mockRepo.Object);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenSalesExist()
        {
            var list = new List<SalesInvoiceMaster> { new() { ID = 1, EntityName = "Customer A", NetAmount = 100m } };
            _mockRepo.Setup(r => r.Get()).ReturnsAsync(list);

            var result = await _controller.Get();

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<List<SalesInvoiceMaster>>(resp.Data);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.Single(data);
            Assert.Equal("Customer A", data[0].EntityName);

            _mockRepo.Verify(r => r.Get(), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var id = 5;
            var item = new SalesInvoiceMaster { ID = id, EntityName = "Customer X" };
            _mockRepo.Setup(r => r.Get(id)).ReturnsAsync(item);

            var result = await _controller.Get(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<SalesInvoiceMaster>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Customer X", data.EntityName);
            _mockRepo.Verify(r => r.Get(id), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsOk_OnSuccess()
        {
            var model = new SalesInvoiceMaster { ID = 0, EntityName = "New" };
            _mockRepo.Setup(r => r.AddEdit(It.IsAny<SalesInvoiceMaster>())).ReturnsAsync(1);

            var result = await _controller.Post(model);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.AddEdit(It.IsAny<SalesInvoiceMaster>()), Times.Once);
        }

        [Fact]
        public async Task Navigate_ReturnsOk_WithNavigateObject()
        {
            var nav = new Navigate { PageNo = 1, RowPerPage = 10, PageCount = 2, Entity = null };
            _mockRepo.Setup(r => r.Navigate(1, 10)).ReturnsAsync(nav);

            var result = await _controller.Navigate(1, 10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<Navigate>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal(1, data.PageNo);
            _mockRepo.Verify(r => r.Navigate(1, 10), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _mockRepo.Setup(r => r.Delete(3)).ReturnsAsync(1);

            var result = await _controller.Delete(3);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.Delete(3), Times.Once);
        }
    }
}
