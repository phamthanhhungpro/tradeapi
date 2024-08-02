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
        Task<UserInfo> GetUserInfoFromToken(string accessToken);
        Task<CudResponseDto> ChangePasswordAsync(ChangePasswordRequest request);
        Task<CudResponseDto> LogoutAsync(Guid userId);

    }

    public class UserServices : IUserServices
    {
        private readonly AppDbContext _dbContext;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly ITokenService _tokenService;

        public UserServices(AppDbContext dbContext, IConfiguration configuration, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _tokenService = tokenService;
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
                Role = request.Role,
                Name = request.Name
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
            await _tokenService.AddToken(token, user.Id);

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
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserInfo> GetUserInfoFromToken(string accessToken)
        {
            return await _dbContext.Tokens
                          .Include(t => t.User).Select(t => new UserInfo
                          {
                              Id = t.User.Id.ToString(),
                              Email = t.User.Email,
                              Role = t.User.Role.ToString(),
                              Name = t.User.Name,
                              TokenString = t.Value,
                              IsValid = t.IsValid
                          }).FirstOrDefaultAsync(t => t.TokenString == accessToken && t.IsValid);
        }

        public async Task<CudResponseDto> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new CudResponseDto { Message = "User not found" };
            }

            if (!VerifyPassword(request.CurrentPassword, user.PassWordHash))
            {
                return new CudResponseDto { Message = "Old password is incorrect" };
            }

            user.PassWordHash = HashPassword(request.NewPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return new CudResponseDto { Message = "Password changed successfully", Id = user.Id };
        }

        public async Task<CudResponseDto> LogoutAsync(Guid userId)
        {
            await _tokenService.DisableAllTokensForUser(userId);
            return new CudResponseDto { Message = "Logged out successfully", Id = userId };
        }
    }
}
