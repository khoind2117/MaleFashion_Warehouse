using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Models.Dtos.OrderItem
{
    public class OrderItemRequestDto
    {
        [Range(1, 10)]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductVariantId { get; set; }
    }
}
