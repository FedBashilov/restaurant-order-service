// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    using Infrastructure.Auth.Extentions;
    using Infrastructure.Database.Extentions;
    using Infrastructure.Menu;
    using Microsoft.OpenApi.Models;
    using Notifications.Service.Extentions;
    using Notifications.Service.Hubs;
    using Orders.Service.Extentions;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseServices(this.Configuration);
            services.AddAuthServices(this.Configuration);
            services.AddMenuServices(this.Configuration);
            services.AddNotificationServices();
            services.AddOrderServices();

            services.AddSignalR();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[] { }
                    },
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<OrderClientHub>("/notifications/client/orders");
                endpoints.MapHub<OrderCookHub>("/notifications/cook/orders");
            });
        }
    }
}
