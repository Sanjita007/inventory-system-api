using inventory_system_api.Application.Models.Reports;

namespace inventory_system_api.Application.IRepository
{
    public interface IDashboardSummaryRepository
    {
        public Task<DashboardSummary> GetDashboardSummary(CancellationToken cancellationToken);
        public Task<SalesPurchSummary?> GetSalesPurchDashboardSummary(CancellationToken cancellationToken);
        public Task<List<ProductSummary>> GetProductDashboardSummary(CancellationToken cancellationToken);
        public Task<List<RecentTransactionSummary>> GetRecentTransactionSummary(CancellationToken cancellationToken);

         
    }
}
