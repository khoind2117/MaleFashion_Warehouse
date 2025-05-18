using MaleFashion_Warehouse.Server.Data;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MaleFashion_Warehouse.Server.Common.Dtos;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<ApplicationDbContext, Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product?> GetProductById(int id)
        {
            return await _dbSet
                .Include(p => p.SubCategory)
                .Include(p => p.ProductVariants!)
                    .ThenInclude(pv => pv.Color)
                .Include(p => p.ProductVariants!)
                    .ThenInclude(pv => pv.Size)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedDto<PagedProductDto>> GetPagedProductsAsync(ProductFilterDto? productFilterDto)
        {
            IQueryable<Product> query = _dbSet;

            query = query.Include(p => p.ProductVariants)
                            .ThenInclude(pv => pv.Size)
                        .Include(p => p.ProductVariants)
                            .ThenInclude(pv => pv.Color)
                        .AsNoTracking();

            if (productFilterDto.MainCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategory != null
                && p.SubCategory.MainCategoryId == productFilterDto.MainCategoryId.Value);
            }

            if (productFilterDto.SubCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategoryId == productFilterDto.SubCategoryId.Value);
            }

            if (productFilterDto.SizeId.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(pv => pv.SizeId == productFilterDto.SizeId));
            }

            if (productFilterDto.ColorId.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(pv => pv.ColorId == productFilterDto.ColorId));
            }

            if (!string.IsNullOrEmpty(productFilterDto.SortBy))
            {
                switch (productFilterDto.SortBy.ToLower())
                {
                    case "new-arrivals":
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                    case "low-to-high":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "high-to-low":
                        query = query.OrderByDescending(p => p.Price);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            var totalRecords = await query.CountAsync();

            var pagedItems = await query.Skip(productFilterDto.GetSkip())
                                        .Take(productFilterDto.GetTake())
                                        .ToListAsync();

            var pagedProductDtos = pagedItems.Select(p => new PagedProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                ColorDtos = p.ProductVariants?
                    .GroupBy(pv => pv.Color)
                    .Select(group => new ColorDto
                    {
                        Id = group.Key.Id,
                        Name = group.Key.Name,
                        ColorCode = group.Key.ColorCode,
                    })
                    .ToList()
            }).ToList();

            return new PagedDto<PagedProductDto>(totalRecords, pagedProductDtos);
        }
    }
}
