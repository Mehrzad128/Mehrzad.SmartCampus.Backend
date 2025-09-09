namespace Mehrzad.SmartCampus.Backend.Students.API.Models
{
    public record UpdateStudentDto(
    string Email,
    string Password,
    string Name,
    string StudentId
);

}
