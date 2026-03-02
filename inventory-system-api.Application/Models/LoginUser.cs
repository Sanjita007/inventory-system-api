using System.ComponentModel.DataAnnotations;

namespace inventory_system_api.Models
{
    public class LoginUser
    {
        
        public required string UserName { get; set; }

        public required string Password { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
