using Dapper.Contrib.Extensions;

namespace inventory_system_api.Application.Models.System
{
    public class Tax
    {
        [Key]
        public int ID { get; set; } 
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        [Computed]
        [Write(false)]
        public string DisplayName => Name + " ("+ Rate + ")";
        public decimal Rate{ get; set; }
    }
}
