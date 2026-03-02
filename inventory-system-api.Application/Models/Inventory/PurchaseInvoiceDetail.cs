namespace inventory_system_api.Application.Models.Inventory
{
    public class PurchaseInvoiceDetail
    {
        public int ID { get; set; }
        public int MasterID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscPercent { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public int QtyUnitID { get; set; }
        public int DefaultUnitID { get; set; }
        public string DefaultUnitName { get; set; }
        public string DefaultUnitSymbol { get; set; }
        public int? TaxID { get; set; }
        public decimal TaxAmount { get; set; }
        public string? GeneralName { get; set; }
        public string? Remarks { get; set; }
        public decimal TotalAmount => NetAmount + TaxAmount;

        public List<UnitDetails> UnitDetails { get; set; } = [];


    }


}
