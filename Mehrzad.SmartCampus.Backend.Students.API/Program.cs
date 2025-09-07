using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Core.Entities;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Students.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
var app = builder.Build();
AppConfiguration.UseServices(app);

// GET all students — Admin & Faculty only
app.MapGet("/students", async (SmartCampusDB db) =>
    await db.Students
        .Include(s => s.User)
        .Select(s => new StudentReadDto
        (
            s.StudentId,
            s.UserId,
            s.User.Name,
            s.User.Email,
            s.EnrollmentDate
        ))
        .ToListAsync()
)
.RequireAuthorization("FacultyAccessLevel");

// GET student by ID — Students can only view themselves
app.MapGet("/students/{id:guid}", async (Guid id, ClaimsPrincipal user, SmartCampusDB db) =>
{
    IResult result;
    var role = user.FindFirstValue(ClaimTypes.Role);
    var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    if (role == "Student")
    {
        // Students can only view themselves
        var student = await db.Students
            .Include(s => s.User)
            .Where(s => s.User.UserId == userId)
            .Select(s => new StudentReadDto(
                s.StudentId,
                s.UserId,
                s.User.Name,
                s.User.Email,
                s.EnrollmentDate
            ))
            .FirstOrDefaultAsync();

        result = student is not null ? Results.Ok(student) : Results.Forbid();

    }
    else
    {
        // Admin/Faculty can view any student
        var student = await db.Students
            .Include(s => s.User)
            .Where(s => s.StudentId == id)
            .Select(s => new StudentReadDto(
                s.StudentId,
                s.UserId,
                s.User.Name,
                s.User.Email,
                s.EnrollmentDate
            ))
            .FirstOrDefaultAsync();

        result = student is not null ? Results.Ok(student) : Results.NotFound();
    }
    return result;
})
.RequireAuthorization("FreeAccessLevel");


// CREATE student — Admin only
app.MapPost("/students", async (StudentCreateDto dto, SmartCampusDB db) =>
{
    IResult result;

    // Load the User from the database
    var user = await db.Users.FindAsync(dto.UserId);
    if (user is null)
    {
        result = Results.BadRequest("User does not exist");
        return result;
    }

    // Create the Student and assign both UserID and User
    var student = new Student
    {
        UserId = dto.UserId,
        User = user,
        EnrollmentDate = DateTime.UtcNow
    };

    db.Students.Add(student);
    await db.SaveChangesAsync();

    var created = new StudentReadDto(
        student.StudentId,
        student.UserId,
        user.Name,
        user.Email,
        student.EnrollmentDate
    );

    result = Results.Created($"/students/{created.StudentId}", created);
    return result;
})
.RequireAuthorization("AdminAccessLevel");


app.MapPut("/students/{id:guid}", async (Guid id, StudentUpdateDto dto, SmartCampusDB db) =>
{
    var student = await db.Students.FindAsync(id);
    if (student is null) return Results.NotFound();

    student.EnrollmentDate = dto.EnrollmentDate;
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.RequireAuthorization("AdminAccessLevel");
// Only Admin can update students


app.MapDelete("/students/{id:guid}", async (Guid id, SmartCampusDB db) =>
{
    var student = await db.Students.FindAsync(id);
    if (student is null) return Results.NotFound();

    db.Students.Remove(student);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.RequireAuthorization("AdminAccessLevel");

app.Run();
