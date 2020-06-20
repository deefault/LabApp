using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Extensions;
using LabApp.Server.Hubs;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using AssignmentService = LabApp.Server.Services.StudentServices.AssignmentService;

namespace LabApp.Server
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();
            services.AddControllers()
                //.AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                ;
            services.AddJwtAuthentication(_env);
            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseLazyLoadingProxies();
                x.UseNpgsql(AppConfiguration.ConnectionString);
            });
            //services.AddMvc();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "LabApp.Angular/dist"; });

            services.AddSwaggerGen(c =>
            {
                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
                //c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("teacher", new OpenApiInfo { Title = "Teacher api", Version = "v1" });
                c.SwaggerDoc("student", new OpenApiInfo { Title = "Student api", Version = "v1" });
                c.SwaggerDoc("common", new OpenApiInfo { Title = "Common api", Version = "v1" });
                c.ResolveConflictingActions(enumerable => enumerable.First());
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet
                });
            });
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 256 * 1024 * 1024;
            });
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddSingleton<PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<GroupService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFileStorage, LocalFileStorage>();
            services.AddScoped<IAuthService, JwtAuthService>();
            services.AddScoped<AttachmentService>();
            services.AddScoped<LessonService>();
            services.AddScoped<AssignmentService>();
            services.AddScoped<HistoryService>();
            services.AddScoped<LabApp.Server.Services.TeacherServices.AssignmentService>();
            services.AddScoped<StudentAssignmentService>();
            services.AddScoped<ConversationService>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, HubUserIdProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.DisplayOperationId();
                x.EnableValidator();
                x.SwaggerEndpoint("/swagger/teacher/swagger.json", "Teacher Api");
                x.SwaggerEndpoint("/swagger/student/swagger.json", "Student Api");
                x.SwaggerEndpoint("/swagger/common/swagger.json", "Common Api");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TeacherHub>("/hubs/teacher"); // TODO
                endpoints.MapControllers();
                //endpoints.MapFallbackToFile("index.html");
            });
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "Frontend/Angular";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
            {
                logger.LogInformation("Migrations started ...");
                context.Database.Migrate();
                logger.LogInformation("Migrations finished.");
            }
        }
    }
}