using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Services.Implementations;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemsService _cartItemService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartItemsController(
            ICartItemsService cartItemService,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartItemService = cartItemService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems(Guid? basketId, string? userId)
        {
            try
            {
                var responseApi = await _cartItemService.GetAllNotPaged(basketId, userId);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
