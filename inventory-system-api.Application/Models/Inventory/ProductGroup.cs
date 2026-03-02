namespace inventory_system_api.Models.Inventory
{
    public class ProductGroup
    {
        public int ID { get; set; } 
        public int ParentGroupID { get; set; } 
        public string ParentGroupName { get; set; } 
        public string EngName { get; set; }
        public string NepName { get; set; }
        public int Level { get; set; }

        public string Remarks { get; set; }
    }
}
