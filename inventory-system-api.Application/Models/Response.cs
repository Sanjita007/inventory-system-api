namespace inventory_system_api.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
    
        public object? Data { get;set; }
    }

    public class CustomResponse<T>
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }

        public T? Data { get; set; }
    }
}
