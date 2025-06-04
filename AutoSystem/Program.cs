using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Domain.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DI;
using Lamar.Microsoft.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Connection
var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AutoSystemDbContext>(options =>
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddIdentity<Auto_Users, IdentityRole>()
    .AddEntityFrameworkStores<AutoSystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// use Lamar as DI.
builder.Host.UseLamar((context, registry) =>
{
    // register services using Lamar
    registry.IncludeRegistry<AutosystemRegistry>();
    registry.IncludeRegistry<MapperRegistry>();
    // add the controllers
});


builder.Services.AddAutoMapper(typeof(GeneralProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", b =>
        b.AllowAnyMethod()
         .AllowAnyHeader()
         .WithOrigins("http://localhost:5173")
         .AllowCredentials());
});

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
