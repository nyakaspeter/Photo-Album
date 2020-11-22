using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Common.Options;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoAlbum.Backend.Web.Helpers
{
    public class JwtHelper
    {
        private UserManager<User> UserManager { get; }
        private JwtOptions JwtOptions { get; }

        public JwtHelper(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
        {
            UserManager = userManager;
            JwtOptions = jwtOptions.Value;
        }

        public static TokenValidationParameters CreateTokenValidationParameters(JwtOptions jwtOptions)
        {
            var signingKey = Convert.FromBase64String(jwtOptions.Key);

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            };
        }

        public async Task<string> GenerateJsonWebToken(LoginDto userDto)
        {
            var claims = new List<Claim>();

            var user = await UserManager.FindByNameAsync(userDto.Username);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var roles = await UserManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var signingKey = Convert.FromBase64String(JwtOptions.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ValidMinutes),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtHandler.WriteToken(jwtToken);

            return token;
        }
    }
}
