using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Dtos.Size;
using MaleFashion_Warehouse.Server.Models.Dtos.SubCategory;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using MaleFashion_Warehouse.Server.Utils;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SlugUtil _slugUtil;

        public ProductService(IUnitOfWork unitOfWork,
            SlugUtil slugUtil)
        {
            _unitOfWork = unitOfWork;
            _slugUtil = slugUtil;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            var productDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                IsActive = product.IsActive
            });

            return productDto;
        }

        public async Task<ProductDetailDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(id);
            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            var productDetailDto = new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                SubCategoryDto = product.SubCategory != null ? new SubCategoryDto
                {
                    Id = product.SubCategory.Id,
                    Name = product.SubCategory.Name,
                    Slug = product.SubCategory.Slug,
                } : null,
                ProductVariantDtos = product.ProductVariants?.Select(pv => new ProductVariantDto
                {
                    Id = pv.Id,
                    Stock = pv.Stock,
                    //ProductId = pv.ProductId,
                    ColorDto = pv.Color != null ? new ColorDto
                    {
                        Id = pv.Color.Id,
                        Name = pv.Color.Name,
                        ColorCode = pv.Color.ColorCode,
                    } : null,
                    SizeDto = pv.Size != null ? new SizeDto
                    {
                        Id = pv.Size.Id,
                        Name = pv.Size.Name,
                    } : null
                }).ToList()
            };

            return productDetailDto;
        }

        public async Task AddAsync(ProductRequestDto productRequestDto)
        {
            var product = new Product
            {
                Name = productRequestDto.Name,
                Slug = _slugUtil.GenerateSlug(productRequestDto.Name),
                Description = productRequestDto.Description,
                Price = productRequestDto.Price,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = false,
                SubCategoryId = productRequestDto.SubCategoryId,
            };

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProductRequestDto productRequestDto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            product.Name = productRequestDto.Name;
            product.Slug = _slugUtil.GenerateSlug(productRequestDto.Name);
            product.Description = productRequestDto.Description;
            product.Price = productRequestDto.Price;
            product.UpdatedAt = DateTime.Now;
            product.SubCategoryId = productRequestDto.SubCategoryId;

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PagedDto<PagedProductDto>> GetPagedAsync(ProductFilterDto? productFilterDto)
        {
            var pagedProducts = await _unitOfWork.ProductRepository.GetPagedProductsAsync(productFilterDto);

            return pagedProducts;
        }
    }
}
