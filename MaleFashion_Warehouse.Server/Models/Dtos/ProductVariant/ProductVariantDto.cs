using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Size;

namespace MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public int Stock { get; set; }

        public ProductDto? ProductDto { get; set; }

        public ColorDto? ColorDto { get; set; }

        public SizeDto? SizeDto { get; set; }
    }
}
