using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.User;
using System.Security.Claims;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseApi<UserDto>> GetUserIdentity(ClaimsPrincipal user);
    }
}
