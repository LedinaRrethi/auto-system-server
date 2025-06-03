using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Entities.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Connection
var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AutoSystemDbContext>(options =>
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AutoSystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
