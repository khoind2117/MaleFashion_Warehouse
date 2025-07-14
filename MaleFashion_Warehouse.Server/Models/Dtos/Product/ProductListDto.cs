using MaleFashion_Warehouse.Server.Models.Dtos.SubCategory;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public string? SubCategoryName { get; set; }
    }
}
