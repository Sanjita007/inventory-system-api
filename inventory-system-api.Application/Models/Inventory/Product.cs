using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.Shared;

namespace inventory_system_api.Models.Inventory
{
    public class Product
    {
        public int ID { get; set; }
        public required string EngName { get; set; } = "";
        public string? NepName { get; set; }
        public int GroupID { get; set; }
        public required string Code { get; set; }

        public string? Remarks { get; set; }
        public int UnitID { get; set; }
        public string? UnitName { get; set; }
        public string? UnitSymbol { get; set; }
        public decimal SalesRate { get; set; } = 0;
        public decimal OpeningQuantity { get; set; } = 0;
        public decimal PurchaseRate { get; set; } = 0;
        public decimal PurchaseDiscount { get; set; } = 0;
        public byte[]? Image { get; set; } = null;
        public string? ImageBase64 => Image != null ? Image?.ToBase64() : null;
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
      
    }

    public class ProductDetails : Product
    {
        public ProductDetails(Product p)
        {
            // Copy all properties from Product into ProductDetails
            ID = p.ID;
            EngName = p.EngName;
            NepName = p.NepName;
            GroupID = p.GroupID;
            Code = p.Code;
            Remarks = p.Remarks;
            UnitID = p.UnitID;
            SalesRate = p.SalesRate;
            OpeningQuantity = p.OpeningQuantity;
            PurchaseRate = p.PurchaseRate;
            PurchaseDiscount = p.PurchaseDiscount;
            Image = p.Image;
            IsBuiltIn = p.IsBuiltIn;
            IsActive = p.IsActive;
            CreatedBy = p.CreatedBy;
            CreatedDate = p.CreatedDate;
            ModifiedBy = p.ModifiedBy;
            ModifiedDate = p.ModifiedDate;
            BackColor = p.BackColor;
            IsVatApplicable = p.IsVatApplicable;
            IsInventoryApplicable = p.IsInventoryApplicable;
            DebtorsID = p.DebtorsID;
            IsDecimalApplicable = p.IsDecimalApplicable;
            ContactPerson = p.ContactPerson;
            Address1 = p.Address1;
            Address2 = p.Address2;
            City = p.City;
            Telephone = p.Telephone;
            Email = p.Email;
            Company = p.Company;
            Website = p.Website;
            CompanyID = p.CompanyID;
            ParentProductID = p.ParentProductID;
            Size = p.Size;
            TaxID = p.TaxID;
        }
        public List<UnitDetails> UnitDetails { get; set; } = [];

    }
}