using MaleFashion_Warehouse.Server.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Color
{
    public class ColorRequestDto
    {
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string Name { get; set; }
        
        [StringLength(CommonConstants.MAX_INPUT)]
        public required string ColorHex { get; set; }
    }
}
