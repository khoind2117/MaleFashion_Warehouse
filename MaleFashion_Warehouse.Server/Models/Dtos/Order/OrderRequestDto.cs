using MaleFashion_Warehouse.Server.Common.Constants;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.OrderItem;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Order
{
    public class OrderRequestDto
    {
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string FirstName { get; set; }
        
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string LastName { get; set; }
        
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string Address {  get; set; } 
        
        public required string PhoneNumber { get; set; }
        
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string Email { get; set; }
        
        [StringLength(CommonConstants.MAX_INPUT_AREA)]
        public string? Note { get; set; }

        public Currency Currency { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public OrderStatus OrderStatus { get; set; }
        
        public string? UserId { get; set; }

        [MinLength(1)]
        public required List<OrderItemRequestDto> OrderItems { get; set; }
    }
}
