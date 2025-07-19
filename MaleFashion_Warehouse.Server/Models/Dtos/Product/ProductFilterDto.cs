using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductFilterDto
    {

        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public List<Category>? Category { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
