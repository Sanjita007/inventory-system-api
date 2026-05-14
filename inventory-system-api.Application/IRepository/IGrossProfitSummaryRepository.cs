using inventory_system_api.Application.Models.Reports;

namespace inventory_system_api.Application.IRepository
{
    public interface IReportRepository
    {
        public Task<GrossProfitSummary> GetGrossProfitReport(CancellationToken cancellationToken);
        public Task<InventorySummary> GetInventoryReport(CancellationToken cancellationToken);
    }
}
