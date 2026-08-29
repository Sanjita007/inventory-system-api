using inventory_system_api.Shared;

namespace inventory_system_api.Application.Models.Reports
{
    public class SalesPurchSummary
    {
        public List<string> Months { get; set; } = [];
        public List<decimal> SalesAmounts { get; set; } = [];
        public List<decimal> PurchAmounts { get; set; } = [];

    }

    public class SalesPurchInitial
    {
        public decimal Sales { get; set; }
        public decimal Purchase { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Date { get; set; } = "";
}

    public class ProductSummary
    {
        public string? ProductName { get; set; }
        public decimal SalesPrice { get; set; }
        public byte[]? ImageByte { get; set; } = null;
        public string? Image => ImageByte != null ? ImageByte?.ToBase64() : null;


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
