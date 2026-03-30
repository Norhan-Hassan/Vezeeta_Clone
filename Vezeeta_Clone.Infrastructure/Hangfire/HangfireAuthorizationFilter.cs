using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vezeeta_Clone.Data.Commons;

namespace Vezeeta_Clone.Infrastructure.Hangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // For development - allow all
            var httpContext = context.GetHttpContext();

            // Allow in development only
            if (httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                return true;
            }

            // Check authentication and admin role
            return httpContext.User?.Identity?.IsAuthenticated == true &&
                   httpContext.User.IsInRole(Roles.Admin);
        }
    }
}
