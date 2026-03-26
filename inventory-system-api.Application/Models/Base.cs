namespace inventory_system_api.Application.Models
{
    public class Base
    {
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CompanyID { get; set; }
        public string? Remarks { get; set; }
    }
}
