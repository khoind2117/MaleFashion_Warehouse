using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class CartItemsRepository : GenericRepository<ApplicationDbContext, CartItem>, ICartItemsRepository
    {
        public CartItemsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
