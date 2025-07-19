using MaleFashion_Warehouse.Server.Common.Dtos;
using System.Linq.Expressions;

namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity?> CreateAsync(TEntity entity);
        Task<bool> CreateManyAsync(IEnumerable<TEntity> entities);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(object id);  
        Task<bool> DeleteManyAsync(IEnumerable<object> ids);
        Task<bool> ChangeStatusAsync<TStatus>(object id, TStatus status)
            where TStatus : struct, Enum;

        Task<TEntity?> GetByIdAsync(
            object id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        );

        Task<List<TEntity>> GetAllNotPagedAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        );

        Task<PageableResponse<TEntity>> GetPagedAsync<TFilter>(
            PagableRequest<TFilter> pagableRequest,
            Func<TFilter?, IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        );
    }
}
