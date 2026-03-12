using Microsoft.Extensions.DependencyInjection;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;
using Vezeeta_Clone.Infrastructure.Repos;

namespace Vezeeta_Clone.Infrastructure
{
    public static class ModuleInfrastructureDependecies
    {
        public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services)
        {

            services.AddTransient<IDoctorRepo, DoctorRepo>();
            services.AddTransient<IPatientRepo, PatientRepo>();
            services.AddTransient<ISpecializationRepo, SpecializationRepo>();
            services.AddTransient<ISubSpecializationRepo, SubSpecializationRepo>();
            services.AddTransient<IRefreshTokenRepo, RefreshTokenRepo>();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }
    }
}
