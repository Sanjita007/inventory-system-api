using accswift_api.Controllers;
using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportRepository> _mockRepo;
        private readonly ReportController _controller;
        private CancellationToken cancellationToken = CancellationToken.None;

        public ReportControllerTests()
        {
            _mockRepo = new Mock<IReportRepository>();
            _controller = new ReportController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetGrossProfitReport_ReturnsOk_WithSummary()
        {
            // Arrange
            var grossList = new List<GrossProfit>
            {
                new GrossProfit { ProductId = 1, ProductCode = "P1", ProductName = "Prod 1", QuantitySold = 2, TotalRevenue = 200, TotalCost = 150, Profit = 50, Margin = 25 },
                new GrossProfit { ProductId = 2, ProductCode = "P2", ProductName = "Prod 2", QuantitySold = 1, TotalRevenue = 100, TotalCost = 60, Profit = 40, Margin = 40 }
            };

            var summary = new GrossProfitSummary
            {
                GrossProfitList = grossList,
                TotalRevenue = 300,
                TotalCost = 210,
                TotalProfit = 90
            };

            _mockRepo.Setup(r => r.GetGrossProfitReport(cancellationToken)).ReturnsAsync(summary);

            // Act
            var result = await _controller.GetGrossProfitReport(cancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<GrossProfitSummary>(resp.Data);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.Equal(2, data.GrossProfitList.Count);
            Assert.Equal(300m, data.TotalRevenue);
            Assert.Equal(210m, data.TotalCost);
            Assert.Equal(90m, data.TotalProfit);

            _mockRepo.Verify(r => r.GetGrossProfitReport(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetInventoryReport_ReturnsOk_WithSummary()
        {
            // Arrange
            var details = new List<InventoryDetail>
            {
                new InventoryDetail { ProductId = 1, ProductCode = "P1", ProductName = "Prod 1", QuantityIn = 10, QuantityOut = 4, QuantityOnHand = 6, TotalInValue = 600 },
                new InventoryDetail { ProductId = 2, ProductCode = "P2", ProductName = "Prod 2", QuantityIn = 5, QuantityOut = 2, QuantityOnHand = 3, TotalInValue = 300 }
            };

            var summary = new InventorySummary
            {
                InventoryDetail = details,
                TotalQuantityIn = 15,
                TotalQuantityOut = 6,
                TotalQuantityOnHand = 9,
                TotalInValue = 900
            };

            _mockRepo.Setup(r => r.GetInventoryReport(cancellationToken)).ReturnsAsync(summary);

            // Act
            var result = await _controller.GetInventoryReport(cancellationToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var resp = Assert.IsType<Models.Response>(ok.Value);
            var data = Assert.IsType<InventorySummary>(resp.Data);

            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(200, resp.StatusCode);
            Assert.Equal("Success", resp.Message);
            Assert.Equal(2, data.InventoryDetail.Count);
            Assert.Equal(15m, data.TotalQuantityIn);
            Assert.Equal(6m, data.TotalQuantityOut);
            Assert.Equal(9m, data.TotalQuantityOnHand);
            Assert.Equal(900m, data.TotalInValue);

            _mockRepo.Verify(r => r.GetInventoryReport(cancellationToken), Times.Once);
        }
    }
}
