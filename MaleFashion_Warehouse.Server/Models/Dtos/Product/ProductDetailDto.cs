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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public SubCategoryDto? SubCategoryDto { get; set; }

        public ICollection<ProductVariantDto>? ProductVariantDtos { get; set; }
    }
}
