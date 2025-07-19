using MaleFashion_Warehouse.Server.Common.Enums;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Order
{
    public class OrderFilterDto
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public string? Address { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public string? Email { get; set; }

        public decimal Total { get; set; }

        public Currency Currency { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
    }
}
