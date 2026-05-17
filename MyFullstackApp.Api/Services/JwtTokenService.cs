using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MyApi.Services;

public sealed class JwtTokenService
{
    private readonly JwtOptions _opt;
    private readonly SymmetricSecurityKey _signingKey;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _opt = options.Value;
        if (string.IsNullOrWhiteSpace(_opt.Secret) || Encoding.UTF8.GetByteCount(_opt.Secret) < 32)
        {
            throw new InvalidOperationException("Jwt:Secret must be at least 32 bytes.");
        }

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Secret));
    }

    public (string AccessToken, string RefreshToken, DateTime AccessExpiresUtc) CreateTokens(int userId, string role, string? displayName, string email)
    {
        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var handler = new JwtSecurityTokenHandler();
        var accessExpires = DateTime.UtcNow.AddMinutes(_opt.AccessMinutes);

        var accessClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new("role", role.Trim().ToLowerInvariant()),
            new(JwtRegisteredClaimNames.Email, email),
            new("name", displayName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, CreateJti()),
        };

        var accessToken = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: accessClaims,
            expires: accessExpires,
            signingCredentials: creds);

        var refreshExpires = DateTime.UtcNow.AddDays(_opt.RefreshDays);
        var refreshClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new("token_kind", "refresh"),
            new(JwtRegisteredClaimNames.Jti, CreateJti()),
        };

        var refreshToken = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: refreshClaims,
            expires: refreshExpires,
            signingCredentials: creds);

        return (
            handler.WriteToken(accessToken),
            handler.WriteToken(refreshToken),
            accessExpires);
    }

    public int? ValidateRefreshTokenAndGetUserId(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(refreshToken, BuildValidationParameters(), out _);
            if (principal.FindFirst("token_kind")?.Value != "refresh")
            {
                return null;
            }

            var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(sub) || !int.TryParse(sub, out var userId))
            {
                return null;
            }

            return userId;
        }
        catch
        {
            return null;
        }
    }

    private TokenValidationParameters BuildValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _signingKey,
            ValidIssuer = _opt.Issuer,
            ValidAudience = _opt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
        };
    }

    private static string CreateJti()
    {
        var bytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
