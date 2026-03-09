using Microsoft.Extensions.DependencyInjection;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Implementation;
using Vezeeta_Clone.Service.Implementation;

namespace Vezeeta_Clone.Service
{
    public static class ModuleServiceDependecies
    {
        public static IServiceCollection AddServiceDependecy(this IServiceCollection services)
        {
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IAutherizationService, AutherizationService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISpecializationService, SpecializationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
