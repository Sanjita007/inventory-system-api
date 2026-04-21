namespace inventory_system_api.Application.Models.System
{
    public class Tax
    {
        public int ID { get; set; } 
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string DisplayName => Name + " ("+ Rate + ")";
        public decimal Rate{ get; set; }
    }
}
