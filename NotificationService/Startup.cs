using AutoMapper;
using LabApp.Shared.EventBus.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Hubs;
using NotificationService.Services;

namespace NotificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostEnvironment _env;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuthentication(_env, Configuration, new []{ "/hubs/teacher", "/hubs/common"});
            services.AddHttpContextAccessor();
            
            services.AddSignalR(x => x.EnableDetailedErrors = _env.IsDevelopment());
            services.AddSingleton<IUserIdProvider, HubUserProvider>();

            services.AddAutoMapper(typeof(Program));
            
            services.AddOptions();
            services.AddControllers().AddNewtonsoftJson();
            services.AddCommon();

            //services.AddScoped<CommonHub>();
            //services.AddScoped<TeacherHub>();

            services.AddScoped<IRealtimeNotificationService, SignalRService>();

            services.AddCors(builder => builder
                .AddPolicy("Policy", x => x 
                    .WithOrigins("http://localhost:4200", "https://localhost:5000", "http://localhost:5001")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials())
            );

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();
            
            app.UseCors("Policy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TeacherHub>("/hubs/teacher").RequireCors("Policy"); // TODO
                endpoints.MapHub<CommonHub>("/hubs/common").RequireCors("Policy");
                endpoints.MapControllers();
                //endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
