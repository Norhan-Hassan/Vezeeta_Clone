using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> _roleManager)
        {
            var rolesCount = await _roleManager.Roles.CountAsync();
            if (rolesCount == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Doctor));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Patient));
            }
        }
    }
}
