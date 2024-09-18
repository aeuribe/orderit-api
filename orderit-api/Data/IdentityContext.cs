using orderit_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace orderit_api.Data
{
    public class IdentityContext: IdentityDbContext  
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    }
}
