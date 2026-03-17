using Microsoft.Extensions.DependencyInjection;
using Vezeeta_Clone.Service.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Abstract;
using Vezeeta_Clone.Service.AppUserAuthServices.Implementation;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;
using Vezeeta_Clone.Service.BackgroundJobServices.Implementation;
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
            services.AddTransient<IClinicService, ClinicService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IDoctorAvailabilityService, DoctorAvailabilityService>();
            services.AddTransient<INotificationJobService, NotificationJobService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IBackgroundJobService, BackgroundJobService>();
            services.AddTransient<ISlotGenerationService, SlotGenerationService>();
            //services.AddScoped<ISlotGenerationJobService, SlotGenerationJobService>();
            return services;
        }
    }
}
