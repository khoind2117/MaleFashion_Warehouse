using MaleFashion_Warehouse.Server.Common.Dtos;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Product
{
    public class ProductFilterDto : FilterBase
    {
        public string? SortBy { get; set; }
        public int? MainCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
    }
}
