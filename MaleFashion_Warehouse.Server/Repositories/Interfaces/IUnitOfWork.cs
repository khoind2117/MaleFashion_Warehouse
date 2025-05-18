namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IProductRepository ProductRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveChangesAsync();
    }
}
