using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Abstractions;
using NotificationService.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static TokenValidationParameters ValidationParameters(IConfiguration conf)
        {
            var parameters = new
                TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf["AppSecret"])),
                    ValidIssuer = conf["JwtIssuer"],
                    ValidateAudience = true,
                    ValidAudience = conf["JwtIssuer"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            parameters.ValidateIssuer = true;
            return parameters;
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IHostEnvironment env,
            IConfiguration conf,
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
                        config.TokenValidationParameters = ValidationParameters(conf);
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
            services.AddSingleton<JwtSecurityTokenHandler>();
        }

        public static IServiceCollection AddNotifiers(this IServiceCollection services)
        {
            services.AddScoped<NotifierProxy>();
            services.AddScoped<INotifier, RealTimeNotifier>();
            services.AddScoped<INotifier, PushNotifier>();

            return services;
        }
    }
}