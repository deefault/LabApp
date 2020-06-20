using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LabApp.Server.Data.Models;
using LabApp.Server.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LabApp.Server.Services
{
    public class JwtAuthService : IAuthService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtAuthService(JwtSecurityTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public static TokenValidationParameters ValidationParameters => new
            TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfiguration.AppSecret)),
                ValidateIssuer = true,
                ValidIssuer = AppConfiguration.JwtIssuer,
                ValidateAudience = true,
                ValidAudience = AppConfiguration.JwtIssuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

        /// <returns>
        ///    Jwt token
        /// </returns>
        public string Authenticate(UserIdentity user)
        {
            DateTime expires = DateTime.UtcNow.AddDays(30);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            JwtSecurityToken jwt = new JwtSecurityToken(
                ValidationParameters.ValidIssuer,
                ValidationParameters.ValidAudience,
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(ValidationParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256)
            );

            return _tokenHandler.WriteToken(jwt);
        }
    }
}