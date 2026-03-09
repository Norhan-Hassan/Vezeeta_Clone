using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.AppUserAuthServices.Abstract
{
    public interface ICurrentUserService
    {
        public Task<ApplicationUser> GetCurrentUserAsync();
        public string GetCurrentUserId();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
