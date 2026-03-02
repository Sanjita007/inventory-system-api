

namespace inventory_system_api.Models.Inventory
{
    public class InvoiceMaster: Base
    {
        
        public string? VoucherNo { get; set; }
        public string EntityName { get; set; } // supplier name for purchase and customer name is for the sale invoice
        public DateTime Date { get; set; }
        public int? ProjectID { get; set; } = 1;
        public decimal TotalQty { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal SpecialDiscount { get;set; }
        public decimal NetAmount { get; set; }   
        public decimal TotalAmount => NetAmount + TotalTCAmount;
        public DateTime SalesDueDate { get; set; }
        public decimal TotalTCAmount { get; set; }
        public decimal TenderAmount { get; set;}
        public decimal ChangeAmount { get; set; }
        public decimal AdjustmentAmount { get;set;}
        public string Status { get; set; } = "PAID";
        public List<InvoiceDetail> Details { get; set; }
       }

    public class SalesInvoiceMaster : InvoiceMaster
    {
    }
}
