using inventory_system_api.Models.Reports;

namespace inventory_system_api.IRepository
{
    public interface IReportRepository
    {
        public Task<GrossProfitSummary> GetGrossProfitReport();
        public Task<InventorySummary> GetInventoryReport();
    }
}
