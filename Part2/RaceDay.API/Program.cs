using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaceDay.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Register controllers.
builder.Services.AddControllers();

// Register the RaceDay database context.
builder.Services.AddDbContext<RaceDayDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("RaceDayConnection")));

// Read JWT configuration from appsettings.json and User Secrets.
var jwtKey = builder.Configuration["Jwt:Key"]
?? throw new InvalidOperationException(
"JWT signing key is missing. Configure Jwt:Key using User Secrets.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
?? throw new InvalidOperationException("JWT issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
?? throw new InvalidOperationException("JWT audience is missing.");

// Register JWT authentication.
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = true,
        ValidAudience = jwtAudience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

// Register authorization.
builder.Services.AddAuthorization();

// Register Swagger/OpenAPI and bearer-token support.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT bearer token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});

});

var app = builder.Build();

// Test the SQL Server database connection.
app.MapGet("/api/health/database", async (RaceDayDbContext db) =>
{
    try
    {
        bool connected = await db.Database.CanConnectAsync();

        if (connected)
        {
            return Results.Ok(new
            {
                status = "Connected",
                database = "RaceDayDB"
            });
        }

        return Results.Problem(
            title: "Database connection failed",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch
    {
        return Results.Problem(
            title: "Unable to connect to the database",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication must run before authorization.
app.UseAuthentication();
app.UseAuthorization();

// Map API controller endpoints.
app.MapControllers();

app.Run();