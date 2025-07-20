using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Generic;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class ProductVariantRepository : GenericRepository<ApplicationDbContext, ProductVariant>, IProductVariantsRepository
    {
        public ProductVariantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
