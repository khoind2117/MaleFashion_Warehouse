namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IProductsRepository ProductRepository { get; }
        IProductVariantsRepository ProductVariantsRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveChangesAsync();
    }
}
