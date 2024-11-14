using Microsoft.IdentityModel.Tokens;
using ProyectoElectivaV.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoElectivaV.Service
{
    public class UtilidadServicio
    {
        private readonly IConfiguration _config;

        public UtilidadServicio(IConfiguration config)
        {
            _config = config;
        }
        public string ConvertirSHA246(string texto)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string GenerarToken(Usuario modelo)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,modelo.id.ToString()),
                new Claim(ClaimTypes.Email, modelo.email!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
