namespace inventory_system_api.Application.Models.Reports
{
    public class GrossProfit
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Profit { get; set; }
        public decimal Margin { get; set; }

    }

    public class GrossProfitSummary
    {
        public List<GrossProfit> GrossProfitList { get; set; } = [];

        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
