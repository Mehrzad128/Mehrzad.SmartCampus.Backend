using Api.Admin.Handlers;
using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Mehrzad.SmartCampus.Backend.Security.API.Handlers;
using Mehrzad.SmartCampus.Backend.Security.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RegisterHandler>();
var app = builder.Build();
AppConfiguration.UseServices(app);
app.UseMiddleware<ExceptionMiddleware>();

//===============================================================================================

app.MapPost("/login", async (LoginDto dto, [FromServices] LoginHandler handler) =>
{
    var (requiresMfa, response) = await handler.HandleLoginAsync(dto);
    if (response is null && !requiresMfa) return Results.Unauthorized();
    if (requiresMfa) return Results.Ok(new { requiresMfa = true , role="Admin" });
    return Results.Ok(response);
});

app.MapPost("/verify-mfa", async (MfaDto dto, LoginHandler handler) =>
{
    var response = await handler.VerifyMfaAsync(dto);
    return response is null ? Results.Unauthorized() : Results.Ok(response);
});

app.MapPost("/register",
    async (RegisterDto dto, [FromServices] RegisterHandler handler) =>
    {
        var response = await handler.HandleAsync(dto);
        if (response is null)
            return Results.BadRequest(new { message = "Registration failed" });

        return Results.Ok(new AuthResponseDto(response));
    });

app.MapGet("/token/validate", (HttpContext ctx) =>
    ValidateTokenHandler.Handle(ctx))
   .AllowAnonymous(); // allow anonymous so frontend can call it even if token is invalid


//===============================================================================================

app.Run();
