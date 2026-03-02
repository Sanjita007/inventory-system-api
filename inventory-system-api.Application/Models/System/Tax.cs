namespace inventory_system_api.Models.System
{
    public class Tax
    {
        public int ID { get; set; } 
        public string Code { get; set; } 
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string DisplayName => Name + " ("+ Rate + ")";
        public decimal Rate{ get; set; }
    }
}
