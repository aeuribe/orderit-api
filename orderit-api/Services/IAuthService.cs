using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Models;

namespace orderit_api.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenStringAndClaims(IdentityUser user, Salesperson salesperson);
        Task<bool> Login(LoginUser user);
        Task<IdentityUser> RegisterUser(LoginUser user);
        Task<List<IdentityUser>> GetAllUsers();
        Task<IdentityUser> GetUserByUsername(string username);
        Task<string> GetRoleForUserAsync(string userId);
    }
}