using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Models.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
