using MaleFashion_Warehouse.Server.Common.Dtos;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductFilterDto
    {

        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? SubCategoryName { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
