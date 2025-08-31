using Mehrzad.SmartCampus.Backend.Core.Entities;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDbContext<SmartCampusDB>(option =>
{
    option.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SmartCampusDB;Trusted_Connection=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();


app.MapPost("/adminlogin", (SmartCampusDB db, AdminLoginDTO adminLogin) =>
{
    if (!db.Admins.Any())
    {
        var firstAdmin = new Admin() { Email = "admin@gmail.com", Name = "admin" };
        db.Admins.Add(firstAdmin);
        db.SaveChanges();
    }
    var result = db.Admins.Where(m => m.Email == adminLogin.Email && m.Name == adminLogin.Name).FirstOrDefault();
    if (result != null)
    {
        return new
        {
            isOK = true,
            message = "Welcome"
        };
    }
    return new
    {
        isOK = false,
        message = "Access denied"
    };
});

app.Run();
