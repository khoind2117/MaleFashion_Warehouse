using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Auth;
using MaleFashion_Warehouse.Server.Models.Dtos.User;
using System.Security.Claims;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseApi<object>> LoginUserAsync(LoginDto loginDto, HttpResponse response);
        Task LogoutAsync(HttpRequest request, HttpResponse response);
    }
}
