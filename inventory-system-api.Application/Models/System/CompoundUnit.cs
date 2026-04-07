namespace inventory_system_api.Application.Models.System
{
    public class CompoundUnit
    {
        public int ID { get; set; }
        public int UnitID { get; set; }
        public int ParentUnitID { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public string ParentUnitName { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public decimal RelationValue { get; set; }
    }
}
