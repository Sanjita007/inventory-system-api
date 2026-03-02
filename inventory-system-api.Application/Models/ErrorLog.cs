namespace inventory_system_api.Application.Models
{
    public class ErrorLog
    {
        public int ID { get; set; }
        public string RequestMethod { get; set; } = string.Empty;
        public string RequestPath { get; set; } = string.Empty;
        public string RequestHeader { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty ;
        public DateTime? DateTimeUtc { get; set; }
    }
}
