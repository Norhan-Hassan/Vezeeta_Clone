using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Vezeeta_Clone.Core;
using Vezeeta_Clone.Core.Middleware;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure;
using Vezeeta_Clone.Infrastructure.Seeder;
using Vezeeta_Clone.Service;
namespace Vezeeta_Clone.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();


            #region Dependency Injection
            builder.Services.AddInfrastructureDependency()
                            .AddServiceDependecy()
                             .AddCoreDependecy().AddServiceRegistrations(builder.Configuration);

            #endregion

            #region Localization
            //builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            builder.Services.AddLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "ar-EG" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);

                // Clear default providers and add only the ones you need
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });
            #endregion

            #region Seeding Database Roles And Admin User
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await RoleSeeder.SeedRolesAsync(roleManager);
                await UserSeeder.SeedUsersAsync(userManager);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Localization Middleware
            var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            #endregion

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
