namespace MaleFashion_Warehouse.Server.Models.Dtos.Auth
{
    public class AccessTokenResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
    }
}
