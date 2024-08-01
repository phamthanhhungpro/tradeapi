using System.Text;
using Microsoft.Extensions.Configuration;
using trade.InfraModel.DataAccess;
using trade.Logic.Requests;
using trade.Shared.Model.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using trade.Logic.Dtos;

namespace trade.Logic.Services
{
    public interface IUserServices
    {
        Task<CudResponseDto> RegisterAsync(RegisterUserRequest request);
        Task<LoginResponseDto> LoginAsync(UserLoginRequest request);
    }

    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public UserServices(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public async Task<CudResponseDto> RegisterAsync(RegisterUserRequest request)
        {
            var existingUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new CudResponseDto { Message = "User already exists", IsSucceeded = false };
            }

            var hashedPassword = HashPassword(request.Password);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PassWordHash = hashedPassword,
                Role = request.Role
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new CudResponseDto { Message = "User registered successfully", Id = newUser.Id };
        }

        public async Task<LoginResponseDto> LoginAsync(UserLoginRequest loginRequest)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.Password, user.PassWordHash))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return new LoginResponseDto
            {
                Token = token,
                User = user
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
