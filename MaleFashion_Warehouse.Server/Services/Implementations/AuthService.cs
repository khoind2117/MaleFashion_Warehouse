using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Auth;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthService(IUnitOfWork unitOfWork,
            ITokenService tokenService,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        private async Task<AuthResponseDto> CreateAuthResponseDtoAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var accessTokenResult = _tokenService.CreateAccessToken(user, roles.ToList());
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(10);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessTokenResult.Token,
                ExpiresIn = accessTokenResult.ExpiresIn,
                RefreshToken = refreshToken,
            };
        }

        public async Task<ResponseApi<object>>LoginUserAsync(LoginDto loginDto, HttpResponse response)
        {
            var responseApi = new ResponseApi<object>();

            var user = await _unitOfWork.AuthRepository.FindByUserNameAsync(loginDto.UserName);
            if (user == null)
            {
                responseApi.Status = 400;
                responseApi.Message = "user_not_found";

                return responseApi;
            };

            var isPasswordValid = await _unitOfWork.AuthRepository.CheckPasswordAsync(user, loginDto.Password);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!isPasswordValid || !userRoles.Contains("Admin"))
            {
                responseApi.Status = 401;
                responseApi.Message = "user_invalid";

                return responseApi;
            };

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(10);
            await _userManager.UpdateAsync(user);
            
            var authResponseDto = await CreateAuthResponseDtoAsync(user);

            responseApi.Status = 200;
            responseApi.Success = true;
            responseApi.Data = authResponseDto;
            responseApi.Message = "login_success";

            return responseApi;
        }

        public async Task LogoutAsync(HttpRequest request, HttpResponse response)
        {
            if (!request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.MinValue;
                await _userManager.UpdateAsync(user);
            }

            response.Cookies.Delete("refreshToken");
        }
    }
}
