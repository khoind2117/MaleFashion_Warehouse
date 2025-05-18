namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(object id);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> ExistsAsync(object id);
        Task<TEntity?> GetByIdAsync(object id);
        Task<IEnumerable<TEntity?>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    }
}
