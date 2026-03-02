namespace inventory_system_api.Models.Reports
{
    public class InventoryDetail
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityIn { get; set; }
        public decimal QuantityOut { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal AverageSalesPrice { get; set; }
        public decimal TotalInValue { get; set; }

    }

    public class InventorySummary
    {
        public List<InventoryDetail> InventoryDetail { get; set; }

        public decimal TotalQuantityIn { get; set; }
        public decimal TotalQuantityOut { get; set; }
        public decimal TotalQuantityOnHand { get; set; }
        public decimal TotalInValue { get; set; }
    }
}
