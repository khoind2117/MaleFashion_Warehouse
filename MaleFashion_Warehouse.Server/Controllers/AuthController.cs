using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Auth;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService,
            ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var responseApi = new ResponseApi<object>
                {
                    Status = 403,
                    Success = false,
                    Message = "service_unauthorized",
                    Data = ModelState,
                };

                return BadRequest(responseApi);
            }

            try
            {
                var responseApi = await _authService.LoginUserAsync(loginDto, Response);
                if (responseApi.Status != 200)
                {
                    return BadRequest(responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = "An error occurred",
                });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync(Request, Response);
                return Ok(new { statusCode = 200, message = "Logout success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = "An error occurred",
                });
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token provided");
            }

            var newAccessToken = await _tokenService.RefreshAccessTokenAsync(refreshToken, Response);

            if (newAccessToken == null)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            return Ok(new { accessToken = newAccessToken });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-token")]
        public async Task<IActionResult> RevokeToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token provided");
            }

            var isRevoked = await _tokenService.RevokeRefreshTokenAsync(refreshToken, Response);

            if (!isRevoked)
            {
                return Unauthorized("Invalid refresh token or token already revoked");
            }

            return Ok("Refresh token revoked successfully");
        }
    }
}
