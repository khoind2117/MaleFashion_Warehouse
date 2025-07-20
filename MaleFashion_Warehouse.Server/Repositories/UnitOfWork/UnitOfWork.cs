using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAuthRepository AuthRepository { get; }
        public IProductsRepository ProductsRepository { get; }
        public IProductVariantsRepository ProductVariantsRepository { get; }
        public IOrdersRepository OrdersRepository { get; }
        public IOrderItemsRepository OrderItemsRepository { get; }
        public ICartItemsRepository CartItemsRepository { get; }
        public IColorsRepository ColorsRepository { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IAuthRepository authRepository,
            IProductsRepository productsRepository,
            IProductVariantsRepository productVariantsRepository,
            IOrdersRepository ordersRepository,
            IOrderItemsRepository ordersItemsRepository,
            ICartItemsRepository cartItemsRepository,
            IColorsRepository colorsRepository
            )
        {
            _context = context;
            AuthRepository = authRepository;
            ProductsRepository = productsRepository;
            ProductVariantsRepository = productVariantsRepository;
            OrdersRepository = ordersRepository;
            OrderItemsRepository = ordersItemsRepository;
            CartItemsRepository = cartItemsRepository;
            ColorsRepository = colorsRepository;
        }

        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitTransactionAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackTransactionAsync() => await _context.Database.RollbackTransactionAsync();
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
