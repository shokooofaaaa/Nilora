using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nilora.Application.DTOS.Auth;
using Nilora.Application.Interfaces;
using Nilora.Domain.Entities;
using Nilora.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Nilora.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly byte[] _signingKey;
        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;

            _signingKey = Convert.FromBase64String(
                _jwtSettings.SigningKey
            );
        }

        public TokenResult GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(_signingKey);

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Role, user.Role.ToString())
         };

            var now = DateTime.UtcNow;

            var expiresAt = now.AddMinutes(_jwtSettings.ExpirationMinutes);
            var token = new JwtSecurityToken(
              issuer: _jwtSettings.Issuer,
              audience: _jwtSettings.Audience,
             claims: claims,
             notBefore: now,
             expires: expiresAt,
             signingCredentials: credentials
             );

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenString = tokenHandler.WriteToken(token);
            return new TokenResult
            {
                Token=tokenString,
                ExpiresAtUtc = expiresAt,
            };
        }
    }
}