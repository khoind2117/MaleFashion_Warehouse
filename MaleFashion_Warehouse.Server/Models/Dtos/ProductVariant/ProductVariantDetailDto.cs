using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;

namespace MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant
{
    public class ProductVariantDetailDto
    {
        public int Id { get; set; }
        
        public int? Stock { get; set; }
        
        public Size Size { get; set; }

        public int? ColorId { get; set; }
        public virtual ColorDto? Color {  get; set; }
    }
}
