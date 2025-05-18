namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class ResponseApi<T>
    {
        public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public int Status { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }
}
