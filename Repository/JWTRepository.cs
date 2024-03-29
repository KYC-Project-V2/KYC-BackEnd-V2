using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class JWTRepository : IJWTRepository
    {
        private readonly IConfiguration _iconfiguration;
        public ISqlConnectionInformation ConnectionInformation { get; set; }
        public JWTRepository(ISqlConnectionInformation sqlConnectionInformation,
            IConfiguration iconfiguration)
        {
            ConnectionInformation = sqlConnectionInformation;
            _iconfiguration = iconfiguration;
        }

        public async Task<Tokens> Authenticate(Authentication users)
        {
            var a = _iconfiguration["JWT:Key"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, users.SecretKey)
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };

        }
    }
}
