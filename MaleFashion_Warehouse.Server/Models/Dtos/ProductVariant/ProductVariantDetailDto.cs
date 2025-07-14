using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Size;

namespace MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant
{
    public class ProductVariantDetailDto
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        
        public int? ColorId { get; set; }
        public ColorDto? Color {  get; set; }

        public int? SizeId { get; set; }
        public SizeDto? Size { get; set; }
    }
}
