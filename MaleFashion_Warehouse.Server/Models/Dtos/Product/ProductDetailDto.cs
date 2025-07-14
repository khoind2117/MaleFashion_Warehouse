using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Dtos.SubCategory;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public int? SubCategoryId { get; set; }
        public SubCategoryDto? SubCategory { get; set; }

        public ICollection<ProductVariantDetailDto>? ProductVariants { get; set; }
    }
}
