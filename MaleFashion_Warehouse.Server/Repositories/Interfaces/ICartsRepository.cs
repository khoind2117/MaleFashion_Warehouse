using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Generic;

namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface ICartsRepository : IGenericRepository<Cart>
    {
        Task<List<Cart>> GetAbandonedCartsAsync(DateTimeOffset beforeTime);
    }
}
