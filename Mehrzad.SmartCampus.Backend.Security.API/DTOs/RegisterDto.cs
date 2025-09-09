using Mehrzad.SmartCampus.Backend.Core.Enums;

namespace Mehrzad.SmartCampus.Backend.Security.API.DTOs
{

    public record RegisterDto(
        string Email,
        string Password,
        string Name,
        UserRole Role,
        string? StudentId,
        string? FacultyId,
        string? Department,
        string? InviteCode
    );
}
