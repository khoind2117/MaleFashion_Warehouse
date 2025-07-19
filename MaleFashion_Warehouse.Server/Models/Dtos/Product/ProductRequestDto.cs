using MaleFashion_Warehouse.Server.Common.Constants;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductRequestDto
    {
        public required string Name { get; set; }
        
        public Category Category { get; set; }

        [Range(1_000, 10_000_000)]
        public required decimal PriceVnd { get; set; }

        [Range(10, 1_000)]
        public decimal? PriceUsd { get; set; }

        [StringLength(CommonConstants.MAX_INPUT_AREA)]
        public string? Description { get; set; }
        
        public ProductStatus Status { get; set; }

        [MinLength(1)]
        public required List<ProductVariantRequestDto> ProductVariants { get; set; }
    }
}
