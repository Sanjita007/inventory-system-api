using accswift_api.Controllers;
using inventory_system_api.Application.IService;
using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Application.Models.System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class PurchaseInvoiceControllerTests
    {
        private readonly Mock<IPurchaseInvoiceMasterService> _mockRepo;
        private readonly PurchaseInvoiceController _controller;
        private CancellationToken cancellationToken = CancellationToken.None;

        public PurchaseInvoiceControllerTests()
        {
            _mockRepo = new Mock<IPurchaseInvoiceMasterService>();
            _controller = new PurchaseInvoiceController(_mockRepo.Object);
            _controller.SetMockUser(userId: 1);

        }

        [Fact]
        public async Task Get_ReturnsOk_WhenPurchaseExist()
        {
            var list = new List<PurchaseInvoiceMaster>
            {
                new PurchaseInvoiceMaster
                {
                    ID = 1,
                    VoucherNo = "P001",
                    EntityName = "Supplier A",
                    Date = DateTime.UtcNow,
                    TotalQty = 5m,
                    GrossAmount = 500m,
                    SpecialDiscount = 0m,
                    NetAmount = 200m,
                    TotalTCAmount = 0m,
                    TenderAmount = 200m,
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
                            Quantity = 5m,
                            Price = 100m,
                            Amount = 500m,
                            DiscPercent = 0m,
                            Discount = 0m,
                            NetAmount = 500m,
                            QtyUnitID = 1,
                            DefaultUnitID = 1,
                            DefaultUnitName = "Piece",
                            DefaultUnitSymbol = "pc",
                            TaxID = null,
                            TaxAmount = 0m,
                            GeneralName = null,
                            Remarks = null
                        }
                    }
                }
            };
            _mockRepo.Setup(r => r.Get(cancellationToken)).ReturnsAsync(list);

            var result = await _controller.Get(cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<List<PurchaseInvoiceMaster>>(resp.Data);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.Single(data);
            Assert.Equal("Supplier A", data[0].EntityName);

            _mockRepo.Verify(r => r.Get(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var id = 7;
            var item = new PurchaseInvoiceMaster
            {
                ID = id,
                VoucherNo = "P007",
                EntityName = "Supplier X",
                Date = DateTime.UtcNow,
                TotalQty = 2m,
                GrossAmount = 200m,
                SpecialDiscount = 0m,
                NetAmount = 150m,
                TotalTCAmount = 0m,
                TenderAmount = 150m,
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
            var data = Assert.IsType<PurchaseInvoiceMaster>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Supplier X", data.EntityName);
            _mockRepo.Verify(r => r.Get(id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsOk_OnSuccess()
        {
            var model = new PurchaseInvoiceMaster
            {
                ID = 0,
                VoucherNo = "PNEW",
                EntityName = "NewSupplier",
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
                Details = new List<InvoiceDetail>()
            };
            _mockRepo.Setup(r => r.AddEdit(It.IsAny<PurchaseInvoiceMaster>(), cancellationToken,1)).ReturnsAsync(1);

            var result = await _controller.Post(model, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.AddEdit(It.IsAny<PurchaseInvoiceMaster>(), cancellationToken,1), Times.Once);
        }

        [Fact]
        public async Task Navigate_ReturnsOk_WithNavigateObject()
        {
            var nav = new Navigate { PageNo = 2, RowPerPage = 5, PageCount = 4, Entity = null };
            _mockRepo.Setup(r => r.Navigate(2, 5, cancellationToken)).ReturnsAsync(nav);

            var result = await _controller.Navigate(2, 5, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<Navigate>(resp.Data);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal(2, data.PageNo);
            _mockRepo.Verify(r => r.Navigate(2, 5, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _mockRepo.Setup(r => r.Delete(4, cancellationToken,1)).ReturnsAsync(1);

            var result = await _controller.Delete(4, cancellationToken);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);

            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            _mockRepo.Verify(r => r.Delete(4, cancellationToken,1), Times.Once);
        }
    }
}
