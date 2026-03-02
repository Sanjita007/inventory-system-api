namespace inventory_system_api.Models.System
{
    public class CompoundUnit
    {
        public int ID { get; set; }
        public int UnitID { get; set; }
        public int ParentUnitID { get; set; }
        public string UnitName { get; set; }
        public string ParentUnitName { get; set; }
        public string Remarks { get; set; }
        public decimal RelationValue { get; set; }
    }
}
