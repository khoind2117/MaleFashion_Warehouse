using MaleFashion_Warehouse.Server.Models.Dtos.Auth;
using MaleFashion_Warehouse.Server.Models.Entities;
using System.Security.Claims;

namespace MaleFashion_Warehouse.Server.Infrastructure.Authentication
{
    public interface ITokenService
    {
        AccessTokenResult CreateAccessToken(User user, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<AuthResponseDto> RefreshAccessTokenAsync(string refreshToken, HttpResponse response);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken, HttpResponse response);
    }
}
