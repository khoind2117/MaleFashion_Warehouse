using MaleFashion_Warehouse.Server.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Entities
{
    public class ProductPrice
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        
        public DateTimeOffset UpdatedDate { get; set; }
        
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
