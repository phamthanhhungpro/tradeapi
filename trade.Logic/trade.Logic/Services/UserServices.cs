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
using trade.Shared.Dtos;
using trade.Shared.Enum;

namespace trade.Logic.Services
{
    public interface IUserServices
    {
        Task<CudResponseDto> RegisterAsync(RegisterUserRequest request);
        Task<LoginResponseDto> LoginAsync(UserLoginRequest request);
        Task<UserInfo> GetUserInfoFromToken(string accessToken);
        Task<CudResponseDto> ChangePasswordAsync(ChangePasswordRequest request);
        Task<CudResponseDto> LogoutAsync(Guid userId);

        Task<PagingResponse<UserDto>> GetPagingUser(PagingRequest request);
        Task<CudResponseDto> DeleteUser(Guid id);
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
                User = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = ((RoleEnum)user.Role).ToString()
                }
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

        public async Task<PagingResponse<UserDto>> GetPagingUser(PagingRequest request)
        {
            var users = await _dbContext.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Role = ((RoleEnum)u.Role).ToString()
                })
                .ToListAsync();

            var totalRecords = await _dbContext.Users.CountAsync();

            return new PagingResponse<UserDto>
            {
                Items = users,
                Count = totalRecords
            };
        }

        public async Task<CudResponseDto> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return new CudResponseDto { Message = "User not found", IsSucceeded = false };
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return new CudResponseDto { Message = "User deleted successfully", Id = user.Id };
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
                return new CudResponseDto { Message = "User not found", IsSucceeded = false };
            }

            if (!VerifyPassword(request.CurrentPassword, user.PassWordHash))
            {
                return new CudResponseDto { Message = "Old password is incorrect", IsSucceeded = false };
            }

            user.PassWordHash = HashPassword(request.NewPassword);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            await _tokenService.DisableAllTokensForUser(user.Id);

            return new CudResponseDto { Message = "Password changed successfully", IsSucceeded = true, Id = user.Id };
        }

        public async Task<CudResponseDto> LogoutAsync(Guid userId)
        {
            return new CudResponseDto { Message = "Logged out successfully", Id = userId };
        }
    }
}
