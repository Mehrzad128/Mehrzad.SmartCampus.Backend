using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
AppConfiguration.AddServices(builder);
var app = builder.Build();
AppConfiguration.UseServices(app);

//===============================================================================================

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
    .RequireAuthorization("AdminAccessLevel");

app.MapGet("/studentList", (SmartCampusDB db) =>
{
    return db.Users.Where(u => u.Role.ToString() == "Student").ToList();
})
    .RequireAuthorization("FacultyAccessLevel");

//===============================================================================================

app.Run();
