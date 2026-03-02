using inventory_system_api.Application.Models.Reports;

namespace inventory_system_api.Application.IRepository
{
    public interface IDashboardSummaryRepository
    {
        public Task<DashboardSummary> GetDashboardSummary();
        public Task<SalesPurchSummary> GetSalesPurchDashboardSummary();
        public Task<List<ProductSummary>> GetProductDashboardSummary();
        public Task<List<RecentTransactionSummary>> GetRecentTransactionSummary();

         
    }
}
