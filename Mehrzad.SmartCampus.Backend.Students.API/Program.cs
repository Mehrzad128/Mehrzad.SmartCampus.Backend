using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Students.API.Models;
using System.Security.Claims;
using Mehrzad.SmartCampus.Backend.Students.API.Handlers;
using Microsoft.AspNetCore.Mvc;
using System;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
builder.Services.AddScoped<StudentCrudHandler>();
var app = builder.Build();
AppConfiguration.UseServices(app);

// List of every student in the system (Admin only)
app.MapGet("/admin/students", async (
    [FromServices] StudentCrudHandler h) =>
{
    var list = await h.GetAllAsync();
    return Results.Ok(list);
})
.RequireAuthorization("AdminAccessLevel");

// CREATE (Admin only)
app.MapPost("/admin/students", async (
    CreateStudentDto dto,
    [FromServices] StudentCrudHandler h) =>
{
    var id = await h.CreateAsync(dto);
    return Results.Created($"/students/{id}", new { id });
})
.RequireAuthorization("AdminAccessLevel");

// READ (role-aware)
app.MapGet("/students/{id:guid}", async (
    Guid id,
    ClaimsPrincipal user,
    [FromServices] StudentCrudHandler h) =>
{
    var student = await h.GetAsync(id, user);
    return student is null ? Results.Forbid() : Results.Ok(student);
})
.RequireAuthorization(); // any authenticated user

// UPDATE (Admin only)
app.MapPut("/admin/students/{id:guid}", async (
    Guid id,
    UpdateStudentDto dto,
    [FromServices] StudentCrudHandler h) =>
{
    var updated = await h.UpdateAsync(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
})
.RequireAuthorization("AdminAccessLevel");

// DELETE (Admin only)
app.MapDelete("/admin/students/{id:guid}", async (
    Guid id,
    [FromServices] StudentCrudHandler h) =>
{
    var deleted = await h.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.RequireAuthorization("AdminAccessLevel");

app.Run();
