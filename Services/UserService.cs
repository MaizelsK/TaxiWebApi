using BCrypt;
using DataAccess;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly TaxiContext _context;
        private readonly IConfiguration _configuration;

        public UserService(TaxiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == login);

            if (user == null || !BCryptHelper.CheckPassword(password, user.PasswordHash))
                return await Task.FromResult<string>(null);

            var tokenHandler = new JwtSecurityTokenHandler();

            var audience = _configuration["JwtSettings:Audience"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

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

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            return token;
        }

        public async Task<User> CreateUser(RegisterUserDto dto)
        {
            // Если ошибок нет, продолжаем создание
            // Иначе выбрасываем исключение для возвращение
            // BadRequest с описанием ошибки исключения
            try { ValidateRegistraionDto(dto); }
            catch (Exception ex)
            {
                throw ex;
            }

            User newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = BCryptHelper.HashPassword(dto.Password, BCryptHelper.GenerateSalt())
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        private bool ValidateRegistraionDto(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                throw new Exception($"Username '{dto.Username}' is already exists");

            if (_context.Users.Any(u => u.Email == dto.Email))
                throw new Exception($"E-mail '{dto.Email}' is already exists");

            if (dto.Role != "client" && dto.Role != "driver")
                throw new Exception("Invalid role");

            return true;
        }
    }
}
