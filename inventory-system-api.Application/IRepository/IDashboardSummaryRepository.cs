using inventory_system_api.Models.Reports;

namespace inventory_system_api.IRepository
{
    public interface IDashboardSummaryRepository
    {
        public Task<DashboardSummary> GetDashboardSummary();
        public Task<SalesPurchSummary> GetSalesPurchDashboardSummary();
        public Task<List<ProductSummary>> GetProductDashboardSummary();
        public Task<List<RecentTransactionSummary>> GetRecentTransactionSummary();

         
    }
}
