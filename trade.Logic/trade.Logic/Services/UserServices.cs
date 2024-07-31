using System.Text;
using Microsoft.Extensions.Configuration;
using trade.InfraModel.DataAccess;
using trade.Logic.Requests;
using trade.Shared.Model.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace trade.Logic.Services
{
    public interface IUserServices
    {
        Task<CudResponseDto> RegisterAsync(RegisterUserRequest request);
        Task<string> LoginAsync(UserLoginRequest request);
    }

    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserServices(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<CudResponseDto> RegisterAsync(RegisterUserRequest request)
        {
            // Check if user already exists
            var existingUser = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new CudResponseDto { Message = "User already exists", IsSucceeded = false };
            }

            // Hash the password
            string passWordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PassWordHash = passWordHash,
                Role = request.Role
            };

            // Save user to database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new CudResponseDto { Message = "User registered successfully", Id = user.Id, IsSucceeded = true };
        }

        public async Task<string> LoginAsync(UserLoginRequest loginDto)
        {
            // Get user by email
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PassWordHash))
            {
                return null;
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()) // Include the user's role
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
