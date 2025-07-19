using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;

namespace MaleFashion_Warehouse.Server.Models.Dtos.CartItem
{
    public class CartItemListDto
    {
        public int Quantity { get; set; }

        public int? ProductVariantId { get; set; }
        public virtual ProductVariantDetailDto? ProductVariant { get; set; }
    }
}
