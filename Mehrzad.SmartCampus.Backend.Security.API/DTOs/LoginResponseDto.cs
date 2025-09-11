namespace Mehrzad.SmartCampus.Backend.Security.API.DTOs
{
    public class LoginResponseDto
    {
        public bool RequiresMfa { get; set; } = false; 
        public string? Token { get; set; } 
        public string? Role { get; set; }
        public string? Message { get; set; } 
    }
}
