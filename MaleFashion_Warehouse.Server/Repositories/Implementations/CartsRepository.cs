using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Generic;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class CartsRepository : GenericRepository<ApplicationDbContext, Cart>, ICartsRepository
    {
        public CartsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<List<Cart>> GetAbandonedCartsAsync(DateTimeOffset beforeTime)
        {
            return _dbSet
                .Where(c => c.UserId == null && c.LastUpdated < beforeTime)
                .ToListAsync();
        }
    }
}
