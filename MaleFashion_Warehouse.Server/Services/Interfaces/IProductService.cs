using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDetailDto> GetByIdAsync(int id);
        Task AddAsync(ProductRequestDto productRequestDto);
        Task UpdateAsync(int id, ProductRequestDto productRequestDto);
        Task<PagedDto<PagedProductDto>> GetPagedAsync(ProductFilterDto? productFilterDto);
    }
}
