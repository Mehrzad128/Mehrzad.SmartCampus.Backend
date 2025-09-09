namespace Mehrzad.SmartCampus.Backend.Students.API.Models
{
    public record ReadStudentDto(
        string StudentId,
        Guid UserId,
        string Name,
        string Email,
        DateTime EnrollmentDate
        );
}