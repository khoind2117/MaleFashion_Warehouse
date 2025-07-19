using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using MaleFashion_Warehouse.Server.Utils;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SlugUtil _slugUtil;

        public ProductsService(IUnitOfWork unitOfWork,
            SlugUtil slugUtil)
        {
            _unitOfWork = unitOfWork;
            _slugUtil = slugUtil;
        }

        public async Task<ResponseApi<ProductDetailDto>> CreateAsync(ProductRequestDto productRequestDto)
        {
            var product = new Product
            {
                Name = productRequestDto.Name,
                Slug = _slugUtil.GenerateSlug(productRequestDto.Name),
                Category = productRequestDto.Category,
                PriceVnd = productRequestDto.PriceVnd,
                PriceUsd = productRequestDto.PriceUsd,
                Description = productRequestDto.Description,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow,
                Status = productRequestDto.Status,
                ProductVariants = productRequestDto.ProductVariants.Select(pv => new ProductVariant
                {
                    Stock = pv.Stock,
                    Size = pv.Size,
                    ColorId = pv.ColorId,
                }).ToList()
            };

            await _unitOfWork.ProductsRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(product.Id);
        }

        public async Task<ResponseApi<object>> UpdateAsync(int id, ProductRequestDto productRequestDto)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found"
                };
            }

            product.Name = productRequestDto.Name;
            product.Slug = _slugUtil.GenerateSlug(productRequestDto.Name);
            product.Category = productRequestDto.Category;
            product.PriceVnd = productRequestDto.PriceVnd;
            product.PriceUsd = productRequestDto.PriceUsd;
            product.Description = productRequestDto.Description;
            product.Status = productRequestDto.Status;
            product.UpdatedDate = DateTimeOffset.UtcNow;

            var existingVariants = product.ProductVariants.ToDictionary(pv => pv.Id);

            foreach (var pvDto in productRequestDto.ProductVariants)
            {
                if (pvDto.Id == null || pvDto.Id == 0)
                {
                    product.ProductVariants.Add(new ProductVariant
                    {
                        Stock = pvDto.Stock,
                        ColorId = pvDto.ColorId,
                        Size = pvDto.Size
                    });
                }
                else if (existingVariants.TryGetValue(pvDto.Id.Value, out var existingVariant))
                {
                    existingVariant.Stock = pvDto.Stock;
                    existingVariant.ColorId = pvDto.ColorId;
                    existingVariant.Size = pvDto.Size;

                    existingVariants.Remove(pvDto.Id.Value);
                }
            }

            foreach (var toRemove in existingVariants.Values)
            {
                await _unitOfWork.ProductVariantsRepository.DeleteAsync(toRemove.Id);
            }

            await _unitOfWork.ProductsRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Updated successfully.",
            };
        }

        public async Task<ResponseApi<object>> DeleteAsync(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found"
                };
            }

            await _unitOfWork.ProductsRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Deleted successfully",
            };
        }

        public async Task<ResponseApi<object>> ChangeStatusAsync(int id, ProductStatus status)
        {
            var isSuccess = await _unitOfWork.ProductsRepository.ChangeStatusAsync(id, status);
            
            if (!isSuccess)
            {
                return new ResponseApi<object>
                {
                    Status = 400,
                    Success = false,
                    Message = "Failed to change status",
                };
            }

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Status updated successfully",
            };
        }


        public async Task<ResponseApi<ProductDetailDto>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetByIdAsync(
                id: id,
                include: p => p.Include(p => p.ProductVariants)
                                    .ThenInclude(pv => pv.Color)
            );

            if (product == null)
            {
                return new ResponseApi<ProductDetailDto>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found",
                };
            }

            var productDetailDto = new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Category = product.Category,
                PriceVnd = product.PriceVnd,
                PriceUsd = product.PriceUsd,
                Description = product.Description,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                Status = product.Status,
                ProductVariants = product.ProductVariants?.Select(pv => new ProductVariantDetailDto
                {
                    Id = pv.Id,
                    Stock = pv.Stock,
                    Size = pv.Size,
                    ColorId = pv.ColorId,
                    Color = pv.Color != null ? new ColorDto
                    {
                        Id = pv.Color.Id,
                        Name = pv.Color.Name,
                        ColorHex = pv.Color.ColorHex,
                    } : new ColorDto(),
                }).ToList()
            };

            return new ResponseApi<ProductDetailDto>
            {
                Status = 200,
                Success = true,
                Data = productDetailDto,
            };
        }

        public async Task<ResponseApi<PageableResponse<ProductListDto>>> GetPaged(PagableRequest<ProductFilterDto> pagableRequest)
        {
            var criteria = pagableRequest.Criteria;
            var page = pagableRequest.Page;
            var size = pagableRequest.Size;
            var sortField = pagableRequest.SortBy;
            var sortDirection = pagableRequest.SortDirection;

            Func<ProductFilterDto?, IQueryable<Product>, IQueryable<Product>>? filter = null;
            if (criteria != null)
            {
                filter = (criteria, q) =>
                {
                    if (!string.IsNullOrWhiteSpace(criteria?.Name))
                        q = q.Where(p => p.Name.Contains(criteria.Name));
                    if (criteria?.Category != null && criteria.Category.Any())
                    {
                        q = q.Where(p => criteria.Category.Contains(p.Category));
                    }
                    if (criteria.UpdatedDate != default)
                        q = q.Where(p => p.UpdatedDate.Date == criteria.UpdatedDate.Date);
                    return q;
                };
            }

            var pagedData = await _unitOfWork.ProductsRepository.GetPagedAsync(
                pagableRequest: pagableRequest,
                filter: filter,
                include: null
            );

            var dtoPaged = new PageableResponse<ProductListDto>
            {
                Content = pagedData.Content?.Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Category = p.Category,
                    PriceVnd = p.PriceVnd,
                    PriceUsd = p.PriceUsd,
                    Description = p.Description,
                    UpdatedDate = p.UpdatedDate,
                    Status = p.Status,
                }).ToList(),
                TotalElements = pagedData.TotalElements,
                TotalPages = pagedData.TotalPages,
                Number = pagedData.Number,
                Size = pagedData.Size,
                NumberOfElements = pagedData.NumberOfElements,
                First = pagedData.First,
                Last = pagedData.Last,
                Empty = pagedData.Empty,
                Sort = pagedData.Sort,
                Pageable = pagedData.Pageable
            };

            return new ResponseApi<PageableResponse<ProductListDto>>
            {
                Status = 200,
                Success = true,
                Data = dtoPaged,
            };
        }
    }
}