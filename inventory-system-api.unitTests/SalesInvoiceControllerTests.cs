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
        private CancellationToken cancellationToken = CancellationToken.None;

        public SalesInvoiceControllerTests()
        {
            _mockRepo = new Mock<ISalesInvoiceService>();
            _controller = new SalesInvoiceController(_mockRepo.Object);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenSalesExist()
        {
            var list = new List<SalesInvoiceMaster>
            {
                new SalesInvoiceMaster
                {
                    ID = 1,
                    VoucherNo = "V001",
                    EntityName = "Customer A",
                    Date = DateTime.UtcNow,
                    TotalQty = 2m,
                    GrossAmount = 120m,
                    SpecialDiscount = 0m,
                    NetAmount = 100m,
                    TotalTCAmount = 0m,
                    TenderAmount = 100m,
                    ChangeAmount = 0m,
                    AdjustmentAmount = 0m,
                    Status = "PAID",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1,
                    CompanyID = 1,
                    Remarks = "unit test",
                    Details = new List<InvoiceDetail>
                    {
                        new InvoiceDetail
                        {
                            ID = 1,
                            MasterID = 1,
                            ProductCode = "P001",
                            ProductName = "Test Product",
                            ProductID = 1,
                            Quantity = 2m,
                            Price = 50m,
                            Amount = 100m,
                            DiscPercent = 0m,
                            Discount = 0m,
                            NetAmount = 100m,
                            QtyUnitID = 1,
                            DefaultUnitID = 1,
                            DefaultUnitName = "Piece",
                            DefaultUnitSymbol = "pc",
                            TaxID = null,
                            TaxAmount = 0m,
                            GeneralName = null,
                            Remarks = null,
                            VATAmount = 0m
                        }
                    }
                }
            };

            _mockRepo.Setup(r => r.Get(cancellationToken)).ReturnsAsync(list);

            var result = await _controller.Get(cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<List<SalesInvoiceMaster>>(resp.Data);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.Single(data);
            Assert.Equal("Customer A", data[0].EntityName);

            _mockRepo.Verify(r => r.Get(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var id = 5;
            var item = new SalesInvoiceMaster
            {
                ID = id,
                VoucherNo = "V010",
                EntityName = "Customer X",
                Date = DateTime.UtcNow,
                TotalQty = 1m,
                GrossAmount = 50m,
                SpecialDiscount = 0m,
                NetAmount = 50m,
                TotalTCAmount = 0m,
                TenderAmount = 50m,
                ChangeAmount = 0m,
                AdjustmentAmount = 0m,
                Status = "PAID",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 1,
                CompanyID = 1,
                Remarks = "unit test",
                Details = new List<InvoiceDetail>()
            };
            _mockRepo.Setup(r => r.Get(id, cancellationToken)).ReturnsAsync(item);

            var result = await _controller.Get(id, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<SalesInvoiceMaster>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Customer X", data.EntityName);
            _mockRepo.Verify(r => r.Get(id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsOk_OnSuccess()
        {
            var model = new SalesInvoiceMaster
            {
                ID = 0,
                VoucherNo = "VNEW",
                EntityName = "New",
                Date = DateTime.UtcNow,
                TotalQty = 0m,
                GrossAmount = 0m,
                SpecialDiscount = 0m,
                NetAmount = 0m,
                TotalTCAmount = 0m,
                TenderAmount = 0m,
                ChangeAmount = 0m,
                AdjustmentAmount = 0m,
                Status = "PAID",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 1,
                CompanyID = 1,
                Remarks = "unit test",
                Details = [
                new InvoiceDetail { ProductID = 1, ProductName = "P1", Quantity = 1, Price = 123.45m, NetAmount = 123.45m }
            ]
            };
            _mockRepo.Setup(r => r.AddEdit(It.IsAny<SalesInvoiceMaster>(), cancellationToken,1)).ReturnsAsync(1);

            var result = await _controller.Post(model, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.AddEdit(It.IsAny<SalesInvoiceMaster>(), cancellationToken,1), Times.Once);
        }

        [Fact]
        public async Task Navigate_ReturnsOk_WithNavigateObject()
        {
            var nav = new Navigate { PageNo = 1, RowPerPage = 10, PageCount = 2, Entity = null };
            _mockRepo.Setup(r => r.Navigate(1, 10, cancellationToken)).ReturnsAsync(nav);

            var result = await _controller.Navigate(1, 10, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<Navigate>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal(1, data.PageNo);
            _mockRepo.Verify(r => r.Navigate(1, 10, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _mockRepo.Setup(r => r.Delete(3, cancellationToken,1)).ReturnsAsync(1);

            var result = await _controller.Delete(3, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.Delete(3, cancellationToken, 1), Times.Once);
        }
    }
}
