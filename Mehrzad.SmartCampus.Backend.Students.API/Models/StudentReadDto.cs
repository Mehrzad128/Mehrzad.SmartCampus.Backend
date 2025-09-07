namespace Mehrzad.SmartCampus.Backend.Students.API.Models
{
    public record StudentReadDto(
        Guid StudentId,
        Guid UserId,
        string Name,
        string Email,
        DateTime EnrollmentDate
        );
}