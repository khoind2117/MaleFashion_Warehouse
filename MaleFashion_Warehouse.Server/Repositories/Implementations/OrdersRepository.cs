using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class OrdersRepository : GenericRepository<ApplicationDbContext, Order>, IOrdersRepository
    {
        public OrdersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
