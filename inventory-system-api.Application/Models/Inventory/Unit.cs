namespace inventory_system_api.Application.Models.Inventory
{
    public class Unit
    {
        public int ID { get; set; } 
        public string Name { get; set; }= string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }

    public class UnitDetails
    {
        public int ID { get; set; }
        public int DefaultUnitID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal ConversionRate { get; set; }
    }
}
