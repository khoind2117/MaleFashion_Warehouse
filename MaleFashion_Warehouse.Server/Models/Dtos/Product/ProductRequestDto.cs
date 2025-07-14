using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductRequestDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int? SubCategoryId { get; set; }

        public required List<ProductVariantRequestDto> ProductVariants { get; set; }
    }
}
