using Microsoft.AspNetCore.Identity;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class AutherizationService : IAutherizationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AutherizationService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> AddRoleAync(string roleName)
        {
            var role = new IdentityRole { Name = roleName };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }



        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> UpdateRoleAync(string id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                role.Name = roleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> DeleteRoleAync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
