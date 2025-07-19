using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        
        public string? Slug { get; set; }
        
        public Category Category { get; set; }

        public decimal? PriceVnd { get; set; }

        public decimal? PriceUsd { get; set; }

        public string? Description { get; set; }
        
        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        
        public ProductStatus Status { get; set; }

        public ICollection<ProductVariantDetailDto>? ProductVariants { get; set; }
    }
}
