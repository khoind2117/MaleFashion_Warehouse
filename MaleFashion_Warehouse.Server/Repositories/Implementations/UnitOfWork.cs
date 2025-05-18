using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAuthRepository AuthRepository { get; }
        public IProductRepository ProductRepository { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IAuthRepository authRepository,
            IProductRepository productRepository)
        {
            _context = context;
            AuthRepository = authRepository;
            ProductRepository = productRepository;
        }

        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitTransactionAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackTransactionAsync() => await _context.Database.RollbackTransactionAsync();
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
