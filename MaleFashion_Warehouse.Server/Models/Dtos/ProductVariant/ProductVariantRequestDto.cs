namespace MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant
{
    public class ProductVariantRequestDto
    {
        public int? Id { get; set; }
        public int Stock { get; set; }
        //public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
    }
}
