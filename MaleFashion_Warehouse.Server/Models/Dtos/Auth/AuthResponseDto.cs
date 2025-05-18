namespace MaleFashion_Warehouse.Server.Models.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
