using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IHostEnvironment env,
            string[] hubPaths = null)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                    {
                        config.RequireHttpsMetadata = !env.IsDevelopment();
                        config.SaveToken = !env.IsDevelopment();
                        config.TokenValidationParameters = JwtAuthService.ValidationParameters;
                        if (hubPaths != null)
                        {
                            config.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    StringValues token = context.Request.Query["access_token"];
                                    PathString path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(token) && hubPaths.Any(x => path.StartsWithSegments(x)))
                                        context.Token = token;

                                    return Task.CompletedTask;
                                }
                            };
                        }
                    }
                );
            services.AddSingleton<IAuthService, JwtAuthService>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            
        }
    }
}