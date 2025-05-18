using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class PagedProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual ICollection<ColorDto>? ColorDtos { get; set; }
    }
}
