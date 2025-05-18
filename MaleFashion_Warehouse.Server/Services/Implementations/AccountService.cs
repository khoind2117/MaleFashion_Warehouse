using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.User;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseApi<UserDto>> GetUserIdentity(ClaimsPrincipal userPrincipal)
        {
            var currentUser = await _userManager.GetUserAsync(userPrincipal);

            if (currentUser == null)
            {
                return new ResponseApi<UserDto>
                {
                    Status = 404,
                    Success = false,
                    Message = "User not found",
                    Error = "User with the provided ID not found",
                };
            }

            var userDto = new UserDto
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                Address = currentUser.Address,
            };

            return new ResponseApi<UserDto>
            {
                Status = 200,
                Success = true,
                Message = "Current user fetched successfully",
                Data = userDto,
            };
        }
    }
}
