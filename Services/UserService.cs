using DataAccess;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaxiWebApi.Helpers;

namespace Services
{
    public class UserService
    {
        private readonly TaxiContext _context;
        private readonly JwtSettings _jwtSettings;

        public UserService(TaxiContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<User> Authenticate(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == login && u.PasswordHash == password);

            if (user == null)
                return await Task.FromResult<User>(null);

            var tokenHandler = new JwtSecurityTokenHandler();

            var audience = _jwtSettings.Audience;
            var issuer = _jwtSettings.Issuer;
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
