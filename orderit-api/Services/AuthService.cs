using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using orderit_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace orderit_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration cofiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = cofiguration;
        }

        public async Task<string> GenerateTokenStringAndClaims(IdentityUser user, Salesperson salesperson)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            SigningCredentials _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            string userRole = await GetRoleForUserAsync(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, userRole ?? string.Empty),
                new Claim("SalespersonId", salesperson.SalespersonId.ToString() ?? string.Empty),
                new Claim("SalespersonName", $"{salesperson?.FirstName} {salesperson?.LastName}")

            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: _signingCredentials
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // Crear un objeto anónimo dinámicamente
            var response = tokenString;

            return response;
        }

        public async Task<List<IdentityUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);

            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(identityUser, user.Password);

        }

        public async Task<IdentityUser> GetUserByUsername(string username)
        {
            return await _userManager.Users
                .Where(s => s.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityUser> RegisterUser(LoginUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName // Asegúrate de que el email esté bien configurado
            };

            try
            {
                // Crear el usuario
                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (!result.Succeeded)
                {
                    // Construir un mensaje de error detallado
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al crear el usuario: {errors}");
                }

                // Asignar rol al usuario
                var roleResult = await _userManager.AddToRoleAsync(identityUser, user.Role);

                if (!roleResult.Succeeded)
                {
                    // Eliminar el usuario si falla la asignación del rol
                    await _userManager.DeleteAsync(identityUser);   
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al asignar el rol al usuario: {errors}");
                }



                return identityUser; // Devuelve el usuario creado
            }
            catch (Exception ex)
            {
                // Logea la excepción para futuras investigaciones
                //_logger.LogError(ex, "Error al registrar al usuario");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior
            }
        }

        public async Task<string> GetRoleForUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault().ToString(); // Obtén el primer rol de la lista
        }
    }
}