using Mehrzad.SmartCampus.Backend.Core.Entities;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<SmartCampusDB>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")
            )
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("FacultyOnly", policy => policy.RequireRole("Faculty"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
});

//---------------------------------------------------------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


//---------------------------------------------------------------------------------------------------------

app.MapPost ("/login", (SmartCampusDB db, LoginDTO login) =>
{
    var result = db.Users.Where(u => u.Email == login.Email && u.Password == login.Password).FirstOrDefault();
    if (result != null)
    {
        var claims = new[] 
        {
            new Claim(JwtRegisteredClaimNames.Sub, login.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, result.Role.ToString())
        };
     
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(3),
            signingCredentials: creds
        );
        return new
        {
            isOK = true,
            message = $"Welcome {result.Name} with role : {result.Role}",
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
    return new
    {
        isOK = false,
        message = "Access denied",
        Token = ""
    };  
});

app.MapGet("/adminList", (SmartCampusDB db) =>
{
    return db.Users.Where(u=> u.Role.ToString()=="Admin").ToList();
})
    .RequireAuthorization("AdminOnly");

app.MapGet("/studentList", (SmartCampusDB db) =>
{
    return db.Users.Where(u => u.Role.ToString() == "Student").ToList();
})
    .RequireAuthorization("AdminOnly");

//---------------------------------------------------------------------------------------------------------

app.Run();
