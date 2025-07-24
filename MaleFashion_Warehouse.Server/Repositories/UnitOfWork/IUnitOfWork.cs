using MaleFashion_Warehouse.Server.Repositories.Interfaces;

namespace MaleFashion_Warehouse.Server.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IProductsRepository ProductsRepository { get; }
        IProductVariantsRepository ProductVariantsRepository { get; }
        IOrdersRepository OrdersRepository { get; }
        IOrderItemsRepository OrderItemsRepository { get; }
        ICartsRepository CartsRepository { get; }
        ICartItemsRepository CartItemsRepository { get; }
        IColorsRepository ColorsRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveChangesAsync();
    }
}
