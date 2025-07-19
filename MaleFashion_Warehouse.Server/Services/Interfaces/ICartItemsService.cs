using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.CartItem;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface ICartItemsService
    {
        Task<ResponseApi<List<CartItemListDto>>> GetAllNotPaged(Guid? basketId, string? userId);
    }
}
