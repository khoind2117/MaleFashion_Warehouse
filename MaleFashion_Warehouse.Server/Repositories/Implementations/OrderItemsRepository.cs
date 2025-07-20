using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Generic;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class OrderItemsRepository : GenericRepository<ApplicationDbContext, OrderItem>, IOrderItemsRepository
    {
        public OrderItemsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
