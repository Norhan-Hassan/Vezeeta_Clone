using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;

namespace Vezeeta_Clone.Service.AppUserAuthServices.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
                                UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetCurrentUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            { throw new UnauthorizedAccessException("Un autherized to access"); }
            return user;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(claim => claim.Type == nameof(AppUserClaimModel.Id))?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("Un autherized to access");
            }
            return userId;
        }
    }
}
