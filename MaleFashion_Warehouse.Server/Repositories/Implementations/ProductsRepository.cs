using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Generic;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class ProductsRepository : GenericRepository<ApplicationDbContext, Product>, IProductsRepository
    {
        public ProductsRepository(ApplicationDbContext context) : base(context) { }

    }
}
