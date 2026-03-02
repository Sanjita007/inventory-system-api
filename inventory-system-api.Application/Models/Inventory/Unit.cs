namespace inventory_system_api.Models.Inventory
{
    public class Unit
    {
        public int ID { get; set; } 
        public string Name { get; set; }
        public string Remarks{ get; set; }
        public string Symbol{ get; set; }
    }

    public class UnitDetails
    {
        public int ID { get; set; }
        public int DefaultUnitID { get; set; }
        public string Name { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
