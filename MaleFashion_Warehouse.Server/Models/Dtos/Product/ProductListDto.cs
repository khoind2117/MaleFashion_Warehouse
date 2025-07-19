using MaleFashion_Warehouse.Server.Common.Enums;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public string? Slug { get; set; }

        public Category Category { get; set; }

        public decimal? PriceVnd { get; set; }

        public decimal? PriceUsd { get; set; }
        
        public string? Description { get; set; }
        
        public DateTimeOffset UpdatedDate { get; set; }
        
        public ProductStatus Status { get; set; }
    }
}
