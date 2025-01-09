using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankTest.API.Models
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }

        public virtual string CreateToken(IdentityUser user, int expirationMinutes)
        {
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // Create JwtPayload with encoded claims and expiration
            var jwtPayload = CreateClaimsPayload(user, expiration);
            var jwtHeader = new JwtHeader(CreateSigningCredentials());

            // Create token with the header and payload
            var token = new JwtSecurityToken(jwtHeader, jwtPayload);

            // Serialize token to a string
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtPayload CreateClaimsPayload(IdentityUser user, DateTime expiration)
        {
            return new JwtPayload(
                _configuration["Jwt:Issuer"], // Issuer
                _configuration["Jwt:Audience"], // Audience
                new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                          new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                          ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
                },
                DateTime.UtcNow,
                expiration // Expiration
            );
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256
            );
        }
    }

}
