using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rest_API.EventHandler;
using Rest_API.Interfaces;
using Rest_API.Models;
using Rest_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Rest_API.EventHandler.LoginHandler;


namespace Rest_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            
            services.AddMvc();
            services.AddDbContext<kreatxTestContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("kreatxTestContext")));


            services.AddScoped<IUserService, UserServices>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITasksServices, TasksServices>();
            services.AddScoped<IProjectsServices, ProjectsServices>();

            services.AddSingleton<IEventBus, EventBus>();
            services.AddTransient<UserLoggedInEventHandler>();

            //services.AddTransient<IEventHandler<UserLoggedInEvent>, UserLoggedInEventHandler>();


            
            //services.AddTransient<IEventHandler<UserLoggedInEvent>, UserLoggedInEventHandler>();
            //services.AddTransient<IEventHandler<UserLoginFailedEvent>, UserLoginFailedEventHandler>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration
                        ["Jwt:Key"]))

                    };




                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireLogin", policy =>
                    policy.RequireAuthenticatedUser());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rest_API", Version = "v1" });


                // Define the security scheme (JWT bearer token)
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                // Add the security scheme to the Swagger document
                c.AddSecurityDefinition("Bearer", securityScheme);

                // Specify the security requirements
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };

                // Add security requirements to operations
                c.AddSecurityRequirement(securityRequirement);


            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rest_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}").RequireAuthorization("RequireLogin");
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
