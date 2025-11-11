using Microsoft.Extensions.DependencyInjection;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.Implementation;

namespace Vezeeta_Clone.Service
{
    public static class ModuleServiceDependecies
    {
        public static IServiceCollection AddServiceDependecy(this IServiceCollection services)
        {
            services.AddTransient<IDoctorService, DoctorService>();
            return services;
        }
    }
}
