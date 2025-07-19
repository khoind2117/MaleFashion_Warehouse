using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.CartItem;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class CartItemsService : ICartItemsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartItemsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseApi<List<CartItemListDto>>> GetAllNotPaged(Guid? basketId, string? userId)
        {
            var cartItems = await _unitOfWork.CartItemsRepository
                .GetAllNotPagedAsync(
                    include: q => q.Include(ci => ci.ProductVariant)
                                        .ThenInclude(pv => pv.Color)
                                    .Include(ci => ci.Cart)
                );

            if (string.IsNullOrEmpty(userId) && basketId == null)
            {
                return new ResponseApi<List<CartItemListDto>>
                {
                    Status = 400,
                    Success = false,
                    Message = "Either userId or basketId must be provided."
                };
            }

            IEnumerable<CartItem> filteredItems = Enumerable.Empty<CartItem>();

            if (!string.IsNullOrEmpty(userId))
            {
                filteredItems = cartItems.Where(ci => ci.Cart.UserId == userId);
            }
            else if (basketId.HasValue)
            {
                filteredItems = cartItems.Where(ci => ci.Cart.BasketId == basketId);
            }

            var cartitemListDto = filteredItems
                .Select(ci => new CartItemListDto
                {
                    Quantity = ci.Quantity,
                    ProductVariantId = ci.ProductVariantId,
                    ProductVariant = new ProductVariantDetailDto
                    {
                        Id = ci.ProductVariant.Id,
                        Stock = null,
                        Size = ci.ProductVariant.Size,
                        ColorId = ci.ProductVariant.ColorId,
                        Color = ci.ProductVariant.Color == null ? null : new ColorDto
                        {
                            Id = ci.ProductVariant.Color.Id,
                            Name = ci.ProductVariant.Color.Name,
                            ColorHex = ci.ProductVariant.Color.ColorHex
                        }
                    }
                }).ToList();

            return new ResponseApi<List<CartItemListDto>>
            {
                Status = 200,
                Success = true,
                Data = cartitemListDto,
            };
        }
    }
}
