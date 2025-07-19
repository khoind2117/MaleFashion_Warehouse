using MaleFashion_Warehouse.Server.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.OrderItem
{
    public class OrderItemDetailDto
    {
        public required string ProductName { get; set; }

        public required string ProductColor { get; set; }

        public Size ProductSize { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Currency Currency { get; set; }

        public int? ProductVariantId { get; set; }

        public int OrderId { get; set; }
    }
}
