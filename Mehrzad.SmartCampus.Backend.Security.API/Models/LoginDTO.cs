using Mehrzad.SmartCampus.Backend.Core.Enums;

namespace Mehrzad.SmartCampus.Backend.Security.API.Models
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
