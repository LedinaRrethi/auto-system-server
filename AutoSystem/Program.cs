using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Domain.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DI;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.OpenApi.Models;
using DAL.Seed;
using System.Text.Json.Serialization;
using Domain.Notifications;
using Microsoft.AspNetCore.Http.Connections;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Connection
var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AutoSystemDbContext>(options =>
{
    options.UseSqlServer(connString);
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddIdentity<Auto_Users, IdentityRole>()
    .AddEntityFrameworkStores<AutoSystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"]
    };

    //See it later TODO
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/Notify"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoSystem", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put Bearer + your token here"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


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

app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/Notify", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
}
);

app.UseHttpsRedirection();

app.MapControllers();


//seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AutoSystemDbContext>();

    await RoleSeeder.SeedRolesAsync(services);
    await UserSeeder.SeedAdminAsync(services);
    await DirectorateSeeder.SeedAsync(context); 
}


app.Run();
