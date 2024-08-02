using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;

namespace trade.Logic.Services;
public interface ITokenService
{
    Task AddToken(string token, Guid userId);
    Task<bool> IsTokenBlacklisted(string token);
    Task DisableAllTokensForUser(Guid userId);
}

public class TokenService : ITokenService
{
    private readonly AppDbContext _context;

    public TokenService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddToken(string token, Guid userId)
    {
        var tokens = new Token
        {
            Value = token,
            UserId = userId
        };

        _context.Tokens.Add(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task DisableAllTokensForUser(Guid userId)
    {
        var tokens = await _context.Tokens.Where(t => t.UserId == userId && t.IsValid).ToListAsync();
        foreach (var token in tokens)
        {
            token.IsValid = false;
        }
        _context.Tokens.UpdateRange(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenBlacklisted(string token)
    {
        return await _context.Tokens.AnyAsync(t => t.Value == token && !t.IsValid);
    }
}

