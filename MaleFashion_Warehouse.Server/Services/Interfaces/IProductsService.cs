using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IProductsService
    {
        //Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ResponseApi<ProductDetailDto>> AddAsync(ProductRequestDto productRequestDto);
        Task<ResponseApi<object>> UpdateAsync(int id, ProductRequestDto productRequestDto);
        Task<ResponseApi<object>> DeleteAsync(int id); 

        Task<ResponseApi<ProductDetailDto>> GetByIdAsync(int id);
        Task<ResponseApi<PageableResponse<ProductListDto>>> GetPaged(PagableRequest<ProductFilterDto> pagableRequest);
    }
}
