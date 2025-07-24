using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaleFashion_Warehouse.Server.Repositories.Generic
{
    public class GenericRepository<TDbContext, TEntity> : IGenericRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly TDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entry.Entity;
        }

        public async Task CreateManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            return true;
        }

        public Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return Task.FromResult(false);
            }

            _dbSet.RemoveRange(entities);
            return Task.FromResult(true);
        }

        public async Task<bool> ChangeStatusAsync<TStatus>(object id, TStatus status)
            where TStatus : struct, Enum
        {
            if (!Enum.IsDefined(typeof(TStatus), status))
            {
                return false;
            }

            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            var statusProp = entity.GetType().GetProperty("Status");
            if (statusProp == null || !statusProp.CanWrite)
                return false;

            statusProp.SetValue(entity, status);
            _context.Entry(entity).State = EntityState.Modified;

            return true;
        }

        public async Task<TEntity?> GetByIdAsync(
            object id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        )
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes
            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task<List<TEntity>> GetAllNotPagedAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<PageableResponse<TEntity>> GetPagedAsync<TFilter>(
            PagableRequest<TFilter> pagableRequest,
            Func<TFilter?, IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        )
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes
            if (include != null)
            {
                query = include(query);
            }

            // Apply filter
            if (filter != null)
            {
                query = filter(pagableRequest.Criteria, query);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(pagableRequest.SortBy))
            {
                var param = Expression.Parameter(typeof(TEntity), "x");
                var property = Expression.PropertyOrField(param, pagableRequest.SortBy);
                var lambda = Expression.Lambda(property, param);

                string methodName = pagableRequest.SortDirection == SortEnum.DESC ? "OrderByDescending" : "OrderBy";

                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName
                             && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TEntity), property.Type);

                query = (IOrderedQueryable<TEntity>)method.Invoke(null, new object[] { query, lambda })!;
            }
            else if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Pagination
            int page = pagableRequest.Page;
            int size = pagableRequest.Size;
            int skip = page * size; // 0-based indexing

            int totalCount = await query.CountAsync();
            var items = await query.Skip(skip).Take(size).ToListAsync();

            return new PageableResponse<TEntity>
            {
                Content = items,
                Empty = !items.Any(),
                First = page == 1,
                Last = skip + items.Count >= totalCount,
                Size = size,
                Number = page,
                NumberOfElements = items.Count,
                Pageable = new Pageable
                {
                    Offset = skip,
                    PageNumber = page,
                    PageSize = size,
                    Paged = true,
                },
                Sort = new Sort
                {
                    Empty = !items.Any(),
                    Sorted = !string.IsNullOrWhiteSpace(pagableRequest.SortBy),
                    Unsorted = string.IsNullOrWhiteSpace(pagableRequest.SortBy),
                },
                TotalElements = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / size)
            };
        }
    }
}
