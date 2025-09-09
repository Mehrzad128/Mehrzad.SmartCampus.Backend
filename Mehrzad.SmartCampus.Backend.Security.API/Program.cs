using Api.Admin.Handlers;
using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Mehrzad.SmartCampus.Backend.Security.API.Handlers;
using Microsoft.AspNetCore.Mvc;
using System;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RegisterHandler>();
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

app.MapPost("/register",
    async (RegisterDto dto, [FromServices] RegisterHandler handler) =>
    {
        var token = await handler.HandleAsync(dto);
        if (token is null)
            return Results.BadRequest(new { message = "Registration failed" });

        return Results.Ok(new AuthResponseDto(token));
    });

//===============================================================================================

app.Run();
