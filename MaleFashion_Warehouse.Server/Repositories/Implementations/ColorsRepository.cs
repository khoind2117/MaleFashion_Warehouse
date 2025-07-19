using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class ColorsRepository : GenericRepository<ApplicationDbContext, Color>, IColorsRepository
    {
        public ColorsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
