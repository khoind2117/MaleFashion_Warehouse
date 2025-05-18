using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserIdentity()
        {
            try
            {
                var response = await _accountService.GetUserIdentity(User);
                if (response.Status == 404)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ResponseApi<object>
                {
                    Status = 401,
                    Success = false,
                    Message = "invalid_token",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = "An error occurred",
                    Error = ex.ToString(),
                });
            }
        }   
    }
}
