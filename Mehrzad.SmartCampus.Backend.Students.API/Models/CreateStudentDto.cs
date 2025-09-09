namespace Mehrzad.SmartCampus.Backend.Students.API.Models
{
    public record CreateStudentDto(
    string Email,
    string Password,
    string Name,
    string StudentId 
);
}