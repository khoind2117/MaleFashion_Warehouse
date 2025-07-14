using Azure.Core;
using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Dtos.Size;
using MaleFashion_Warehouse.Server.Models.Dtos.SubCategory;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Implementations;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using MaleFashion_Warehouse.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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

        #region CRUD
        public async Task<ResponseApi<ProductDetailDto>> AddAsync(ProductRequestDto productRequestDto)
        {
            var product = new Product
            {
                Name = productRequestDto.Name,
                Slug = _slugUtil.GenerateSlug(productRequestDto.Name),
                Description = productRequestDto.Description,
                Price = productRequestDto.Price,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                SubCategoryId = productRequestDto.SubCategoryId,
                ProductVariants = productRequestDto.ProductVariants.Select(pv => new ProductVariant
                {
                    Stock = pv.Stock,
                    ColorId = pv.ColorId,
                    SizeId = pv.SizeId
                }).ToList()
            };

            await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(product.Id);
        }

        public async Task<ResponseApi<object>> UpdateAsync(int id, ProductRequestDto productRequestDto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
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
            product.Description = productRequestDto.Description;
            product.Price = productRequestDto.Price;
            product.UpdatedAt = DateTime.Now;
            product.SubCategoryId = productRequestDto.SubCategoryId;

            var existingVariants = product.ProductVariants.ToDictionary(pv => pv.Id);

            foreach (var pvDto in productRequestDto.ProductVariants)
            {
                if (pvDto.Id == null || pvDto.Id == 0)
                {
                    product.ProductVariants.Add(new ProductVariant
                    {
                        Stock = pvDto.Stock,
                        ColorId = pvDto.ColorId,
                        SizeId = pvDto.SizeId,
                    });
                }
                else if (existingVariants.TryGetValue(pvDto.Id.Value, out var existingVariant)) 
                {
                    existingVariant.Stock = pvDto.Stock;
                    existingVariant.ColorId = pvDto.ColorId;
                    existingVariant.SizeId = pvDto.SizeId;

                    existingVariants.Remove(pvDto.Id.Value);
                }
            }

            foreach (var toRemove in existingVariants.Values)
            {
                await _unitOfWork.ProductVariantsRepository.DeleteAsync(toRemove.Id);
            }

            await _unitOfWork.ProductRepository.UpdateAsync(product);
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
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found"
                };
            }

            await _unitOfWork.ProductRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Deleted successfully",
            };
        }
        #endregion

        public async Task<ResponseApi<ProductDetailDto>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(
                id: id,
                include: p => p.Include(p => p.SubCategory)
                                .Include(p => p.ProductVariants)
                                    .ThenInclude(pv => pv.Color)
                                .Include(p => p.ProductVariants)
                                    .ThenInclude(pv => pv.Size)
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
                Description = product.Description,
                Price = product.Price,
                UpdatedAt = product.UpdatedAt,
                IsActive = product.IsActive,
                SubCategoryId = product.SubCategoryId,
                SubCategory = product.SubCategory != null ? new SubCategoryDto
                {
                    Id = product.SubCategory.Id,
                    Name = product.SubCategory.Name,
                } : new SubCategoryDto(),
                ProductVariants = product.ProductVariants?.Select(pv => new ProductVariantDetailDto
                {
                    Id = pv.Id,
                    Stock = pv.Stock,
                    ColorId = pv.ColorId,
                    Color = pv.Color != null ? new ColorDto
                    {
                        Id = pv.Color.Id,
                        Name = pv.Color.Name,
                        ColorCode = pv.Color.ColorCode,
                    } : new ColorDto(),
                    SizeId = pv.SizeId,
                    Size = pv.Size != null ? new SizeDto
                    {
                        Id = pv.Size.Id,
                        Name = pv.Size.Name,
                    } : new SizeDto(),
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
                    if (criteria.Price.HasValue)
                        q = q.Where(p => p.Price == criteria.Price);
                    if (!string.IsNullOrWhiteSpace(criteria?.SubCategoryName))
                        q = q.Where(p => p.Name.Contains(criteria.SubCategoryName));
                    if (criteria.UpdatedAt != default)
                        q = q.Where(p => p.UpdatedAt.Date == criteria.UpdatedAt.Date);
                    return q;
                };
            }

            var pagedData = await _unitOfWork.ProductRepository.GetPagedAsync(
                pagableRequest: pagableRequest,
                filter: filter,
                include: p => p.Include(p => p.SubCategory)
            );

            var dtoPaged = new PageableResponse<ProductListDto>
            {
                Content = pagedData.Content?.Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    UpdatedAt = p.UpdatedAt,
                    IsActive = p.IsActive,
                    SubCategoryName = p.SubCategory?.Name
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
