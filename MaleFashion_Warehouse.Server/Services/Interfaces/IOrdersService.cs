

using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Order;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IOrdersService
    {
        //Task<ResponseApi<OrderDetailDto>> CreateAsync(OrderRequestDto productRequestDto);
        Task<ResponseApi<object>> UpdateAsync(int id, OrderRequestDto productRequestDto);
        Task<ResponseApi<object>> DeleteAsync(int id);
        Task<ResponseApi<object>> ChangeStatusAsync(int id, OrderStatus status);

        Task<ResponseApi<OrderDetailDto>> GetByIdAsync(int id);
        Task<ResponseApi<PageableResponse<OrderListDto>>> GetPaged(PagableRequest<OrderFilterDto> pagableRequest);
    }
}
