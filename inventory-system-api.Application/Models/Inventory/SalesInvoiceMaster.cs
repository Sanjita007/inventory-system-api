

namespace inventory_system_api.Application.Models.Inventory
{
    public class InvoiceMaster: Base
    {
        
        public string? VoucherNo { get; set; }
        public string EntityName { get; set; } // supplier name for purchase and customer name is for the sale invoice
        public DateTime Date { get; set; }
        public int? ProjectID { get; set; } = 1;
        public decimal TotalQty { get; set; } = 0;
        public decimal GrossAmount { get; set; } = 0;
        public decimal SpecialDiscount { get; set; } = 0;
        public decimal NetAmount { get; set; } = 0;
        public decimal TotalAmount => NetAmount + TotalTCAmount;
        public DateTime? SalesDueDate { get; set; }
        public decimal TotalTCAmount { get; set; } = 0;
        public decimal TenderAmount { get; set; } = 0;
        public decimal ChangeAmount { get; set; } = 0;
        public decimal AdjustmentAmount { get; set; } = 0;
        public string Status { get; set; } = "PAID";
        public List<InvoiceDetail> Details { get; set; }
       }

    public class SalesInvoiceMaster : InvoiceMaster
    {
    }
}
