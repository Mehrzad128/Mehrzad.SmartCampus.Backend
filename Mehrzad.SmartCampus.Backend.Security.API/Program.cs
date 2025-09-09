using Api.Admin.Handlers;
using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
builder.Services.AddScoped<LoginHandler>();
var app = builder.Build();
AppConfiguration.UseServices(app);

//===============================================================================================

app.MapPost("/login", async (LoginDto dto, [FromServices] LoginHandler handler) =>
{
    var (requiresMfa, token) = await handler.HandleLoginAsync(dto);
    if (token is null && !requiresMfa) return Results.Unauthorized();
    if (requiresMfa) return Results.Ok(new { requiresMfa = true });
    return Results.Ok(new { token });
});

app.MapPost("/verify-mfa", async (MfaDto dto, LoginHandler handler) =>
{
    var token = await handler.VerifyMfaAsync(dto);
    return token is null ? Results.Unauthorized() : Results.Ok(new { token });
});

//===============================================================================================

app.Run();
