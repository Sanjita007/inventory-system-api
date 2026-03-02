namespace inventory_system_api.Middleware
{
    internal class ErrorResponse
    {
        internal Dictionary<string, string[]> Errors;

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
    }
}