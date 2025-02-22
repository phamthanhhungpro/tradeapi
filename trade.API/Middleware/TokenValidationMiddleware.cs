using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
namespace trade.API.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext _dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/api/User/login"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var isBlackListToken = await _dbContext.Tokens.AnyAsync(x => x.Value == token && !x.IsValid);

            if (isBlackListToken)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized request: Token is blacklisted");
                _logger.LogInformation("Unauthorized request: Token is blacklisted");
                return;
            }

            await _next(context);
        }
    }
}