using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
using trade.Logic.Services;
using FluentValidation.AspNetCore;
using trade.API.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

var builder = WebApplication.CreateBuilder(args);

// Add JWT authentication services
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? string.Empty,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? string.Empty,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
builder.Services.AddTransient<IGenericValidator, GenericValidator>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://example.com") // Replace with your allowed origin
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure this middleware is added before Authorization

app.UseAuthorization();

// Apply the CORS policy
app.UseCors("AllowSpecificOrigin");

app.MapControllers();
app.Run();