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
using trade.Logic.Interfaces;
using Microsoft.OpenApi.Models;

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
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();
//builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////builder.Services.AddSwaggerGen(c =>
////{
////    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

////    // Add JWT Authentication
////    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
////    {
////        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
////        Name = "Authorization",
////        In = ParameterLocation.Header,
////        Type = SecuritySchemeType.ApiKey,
////        Scheme = "Bearer"
////    });

////    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
////    {
////        {
////            new OpenApiSecurityScheme
////            {
////                Reference = new OpenApiReference
////                {
////                    Type = ReferenceType.SecurityScheme,
////                    Id = "Bearer"
////                },
////                Scheme = "oauth2",
////                Name = "Bearer",
////                In = ParameterLocation.Header,
////            },
////            new List<string>()
////        }
////    });
////});


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Apply the CORS policy
app.UseCors("AllowAllOrigin");

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure this middleware is added before Authorization

app.UseAuthorization();

app.MapControllers();
app.Run();