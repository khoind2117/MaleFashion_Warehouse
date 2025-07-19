using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant
{
    public class ProductVariantRequestDto
    {
        public int? Id { get; set; }

        [Range(1, 1000)]
        public int Stock { get; set; }
        
        public Size Size { get; set; }

        public int ColorId { get; set; }
        public virtual ColorRequestDto? Color { get; set; }
    }
}
