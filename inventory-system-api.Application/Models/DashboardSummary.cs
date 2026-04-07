namespace inventory_system_api.Application.Models.Reports
{
    public class SalesPurchSummary
    {
        public List<string> Months { get; set; } = [];
        public List<decimal> SalesAmounts { get; set; } = [];
        public List<decimal> PurchAmounts { get; set; } = [];

    }

    public class ProductSummary
    {
        public string? ProductName { get; set; }
        public decimal SalesPrice { get; set; }
        public string? Image { get; set; }

    }

    public class RecentTransactionSummary
    {
        public string? Date { get; set; }
        public string? Details { get; set; }
    }

    public class DashboardSummary
    {
        public SalesPurchSummary? SalesPurch { get; set; }
        public List<ProductSummary>? Product { get; set; }
    }

}
