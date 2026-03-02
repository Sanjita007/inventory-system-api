using inventory_system_api.Models.Inventory;

namespace inventory_system_api.Models.Inventory
{
    public class Product
    {
        public int ID { get; set; }
        public string EngName { get; set; }
        public string NepName { get; set; }
        public int GroupID { get; set; }
        public string Code { get; set; }

        public int DepotID { get; set; }
        public string? Remarks { get; set; }
        public int UnitID { get; set; }
        public string? UnitName { get; set; }
        public string? UnitSymbol { get; set; }
        public decimal SalesRate { get; set; } = 0;
        public decimal PurchaseQuantity { get; set; } = 0;
        public decimal PurchaseRate { get; set; } = 0;
        public decimal PurchaseDiscount { get; set; } = 0;
        public decimal TotalValue { get; set; } = 0;
        public string? Image { get; set; } = null;
        public bool IsBuiltIn { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; } = "root";
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? BackColor { get; set; }
        public bool IsVatApplicable { get; set; } = false;
        public bool IsInventoryApplicable { get; set; } = false;
        public int? DebtorsID { get; set; }
        public DateTime? RentDate { get; set; }
        public decimal Quantity { get; set; } = 0;
        public bool IsDecimalApplicable { get; set; } = false;
        public string? ContactPerson { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public int CompanyID { get; set; } = 1;
        public string? ParentProductID { get; set; }
        public string? Size { get; set; }
        public int? TaxID { get; set; }
        public decimal ConversionRate { get; set; } = 1;

        //public UnitDetails[] UnitDetails { get; set; }
    }

    public class ProductDetails : Product
    {
        public ProductDetails(Product p)
        {
            // Copy all properties from Product into ProductDetails
            this.ID = p.ID;
            this.EngName = p.EngName;
            this.NepName = p.NepName;
            this.GroupID = p.GroupID;
            this.Code = p.Code;
            this.DepotID = p.DepotID;
            this.Remarks = p.Remarks;
            this.UnitID = p.UnitID;
            this.SalesRate = p.SalesRate;
            this.PurchaseQuantity = p.PurchaseQuantity;
            this.PurchaseRate = p.PurchaseRate;
            this.PurchaseDiscount = p.PurchaseDiscount;
            this.TotalValue = p.TotalValue;
            this.Image = p.Image;
            this.IsBuiltIn = p.IsBuiltIn;
            this.IsActive = p.IsActive;
            this.CreatedBy = p.CreatedBy;
            this.CreatedDate = p.CreatedDate;
            this.ModifiedBy = p.ModifiedBy;
            this.ModifiedDate = p.ModifiedDate;
            this.BackColor = p.BackColor;
            this.IsVatApplicable = p.IsVatApplicable;
            this.IsInventoryApplicable = p.IsInventoryApplicable;
            this.DebtorsID = p.DebtorsID;
            this.RentDate = p.RentDate;
            this.Quantity = p.Quantity;
            this.IsDecimalApplicable = p.IsDecimalApplicable;
            this.ContactPerson = p.ContactPerson;
            this.Address1 = p.Address1;
            this.Address2 = p.Address2;
            this.City = p.City;
            this.Telephone = p.Telephone;
            this.Email = p.Email;
            this.Company = p.Company;
            this.Website = p.Website;
            this.CompanyID = p.CompanyID;
            this.ParentProductID = p.ParentProductID;
            this.Size = p.Size;
            this.TaxID = p.TaxID;
        }
        public List<UnitDetails> UnitDetails { get; set; } = [];

    }
}