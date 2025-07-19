namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IProductsRepository ProductsRepository { get; }
        IProductVariantsRepository ProductVariantsRepository { get; }
        IOrdersRepository OrdersRepository { get; }
        IOrderItemsRepository OrderItemsRepository { get; }
        ICartItemsRepository CartItemsRepository { get; }
        IColorsRepository ColorsRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveChangesAsync();
    }
}
