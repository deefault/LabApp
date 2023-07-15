using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Hubs;
using LabApp.Server.Infrastructure.Swagger.Filters;
using LabApp.Server.Services;
using LabApp.Server.Services.ImageService;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Services.TeacherServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddControllers();
            services.AddJwtAuthentication(_env);


            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseLazyLoadingProxies();
                x.UseNpgsql(AppConfiguration.ConnectionString);
            });
            //services.AddMvc();
            //services.(configuration => { configuration.RootPath = "LabApp.Angular/dist"; });

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<AutoRestSchemaFilter>();
                c.CustomOperationIds(apiDesc =>
                    apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
                //c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("teacher", new OpenApiInfo {Title = "Teacher api", Version = "v1"});
                c.SwaggerDoc("student", new OpenApiInfo {Title = "Student api", Version = "v1"});
                c.SwaggerDoc("common", new OpenApiInfo {Title = "Common api", Version = "v1"});
                c.ResolveConflictingActions(enumerable => enumerable.First());
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddResponseCompression(options => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[]
                {
                    MediaTypeNames.Application.Octet
                }));
            services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 256 * 1024 * 1024);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, HubUserIdProvider>();

            services.AddSingleton<PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<GroupService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IFileStorage, LocalFileStorage>();
            services.AddScoped<IAuthService, JwtAuthService>();
            services.AddScoped<AttachmentService>();
            services.AddScoped<LessonService>();
            services.AddScoped<AssignmentService>();
            services.AddScoped<HistoryService>();
            services.AddScoped<LabApp.Server.Services.TeacherServices.AssignmentService>();
            services.AddScoped<StudentAssignmentService>();
            services.AddScoped<ConversationService>();
            services.AddScoped<CommonHub>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddEventBusWithConsistency(Configuration);

            services.AddCors(builder => builder
                .AddPolicy("Policy", x => x
                    .WithOrigins("http://localhost:4200", "https://localhost:5002", "http://localhost:5001",
                        "http://localhost:80", "https://localhost:443")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials())
            );
        }

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
            app.UseReDoc(x =>
            {
                x.RoutePrefix = "api-docs";
                x.ExpandResponses("200, 201");
                x.SpecUrl("/swagger/teacher/swagger.json");
                x.SpecUrl("/swagger/student/swagger.json");
                x.SpecUrl("/swagger/common/swagger.json");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseBlazorFrameworkFiles();
            /*app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(_env.ContentRootPath, "wwwroot/dist")),
                    new PhysicalFileProvider("wwwroot/dist")
                    )
            });*/
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("Policy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}