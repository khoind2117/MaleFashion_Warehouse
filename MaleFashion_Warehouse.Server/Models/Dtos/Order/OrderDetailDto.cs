using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.OrderItem;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Order
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }

        public string? Note { get; set; }

        public decimal Total { get; set; }

        public Currency Currency { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string? UserId { get; set; }

        public virtual ICollection<OrderItemDetailDto> OrderItems { get; set; } = new List<OrderItemDetailDto>();
    }
}
