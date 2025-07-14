using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using System.Linq.Expressions;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class ProductsRepository : GenericRepository<ApplicationDbContext, Product>, IProductsRepository
    {
        public ProductsRepository(ApplicationDbContext context) : base(context) { }

    }
}
