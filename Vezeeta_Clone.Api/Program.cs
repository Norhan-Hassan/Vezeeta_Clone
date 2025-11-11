using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Core;
using Vezeeta_Clone.Core.Middleware;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Service;
namespace Vezeeta_Clone.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            #region Identity
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            #endregion


            #region Dependency Injection
            builder.Services.AddInfrastructureDependency()
                            .AddServiceDependecy()
                             .AddCoreDependecy().AddServiceRegistrations();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
