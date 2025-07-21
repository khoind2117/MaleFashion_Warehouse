using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IProductsService
    {
        Task<ResponseApi<ProductDetailDto>> CreateAsync(ProductRequestDto productRequestDto);
        Task<ResponseApi<object>> UpdateAsync(int id, ProductRequestDto productRequestDto);
        Task<ResponseApi<object>> DeleteAsync(int id);
        Task<ResponseApi<object>> ChangeStatusAsync(int id, ProductStatus status);

        Task<ResponseApi<ProductDetailDto>> GetByIdAsync(int id);
        Task<ResponseApi<PageableResponse<ProductListDto>>> GetPaged(PagableRequest<ProductFilterDto> pagableRequest);
    }
}
