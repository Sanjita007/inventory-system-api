namespace inventory_system_api.Application.Models
{
    public class ErrorResponse
    {
        public Dictionary<string, string[]> Errors;

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
    }
}