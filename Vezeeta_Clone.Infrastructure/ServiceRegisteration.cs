using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Infrastructure.Context;

namespace Vezeeta_Clone.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            //JWT Authentication
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
            //Email Settings
            var emailSettings = new EmailSettings();
            configuration.GetSection(nameof(EmailSettings)).Bind(emailSettings);
            //stripe payment
            var stripeSettings = new StripeSettings();
            configuration.GetSection(nameof(StripeSettings)).Bind(stripeSettings);

            services.AddSingleton(jwtSettings);
            services.AddSingleton(emailSettings);
            services.AddSingleton(stripeSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(options =>
            {
                var clientId = configuration["Authentication:Google:ClientId"];
                if (clientId == null)
                {
                    throw new ArgumentNullException(nameof(clientId));
                }
                var clientSecret = configuration["Authentication:Google:ClientSecret"];

                if (clientSecret == null)
                {
                    throw new ArgumentNullException(nameof(clientSecret));
                }
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = jwtSettings.ValidateIssuer,
                      ValidIssuers = new[] { jwtSettings.Issuer },
                      ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                      ValidAudience = jwtSettings.Audience,
                      ValidateAudience = jwtSettings.ValidateAudience,
                      ValidateLifetime = jwtSettings.ValidateLifeTime,
                      RoleClaimType = ClaimTypes.Role
                  };
              });

            // Swagger configuration
            services.AddSwaggerGen(options =>
            {
                // API Info

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vezeeta Clone",
                    Version = "v1",
                    Description = "Initial version"
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Vezeeta Clone",
                    Version = "v2",
                    Description = "Updated version with new features"
                });

                options.EnableAnnotations();

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Enter JWT Token without bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    BearerFormat = "JWT"
                });

                //   options.SchemaFilter<EnumSchemaFilter>();
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                                Reference= new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }

                        },
                        Array.Empty<string>()
                    }
                });
            });

            ////google auth
            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //});

            // API Versioning
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });


            return services;
        }
    }

    //public class EnumSchemaFilter : ISchemaFilter
    //{
    //    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    //    {
    //        if (context.Type.IsEnum)
    //        {
    //            schema.Enum.Clear();
    //            schema.Type = "string";

    //            foreach (var enumValue in Enum.GetNames(context.Type))
    //            {
    //                schema.Enum.Add(new OpenApiString(enumValue));
    //            }
    //        }
    //    }
    //}


}
