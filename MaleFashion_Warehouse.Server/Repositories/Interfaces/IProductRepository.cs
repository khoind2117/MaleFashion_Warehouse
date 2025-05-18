using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Entities;

namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetProductById(int id);
        Task<PagedDto<PagedProductDto>> GetPagedProductsAsync(ProductFilterDto? productFilterDto);
    }
}
