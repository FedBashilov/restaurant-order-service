// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    using Infrastructure.Menu;
    using Web.Facade.Extentions;
    using Web.Facade.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMenuServices();
            services.AddOrderServices(this.Configuration);
            services.AddAuthServices(this.Configuration);

            services.AddSignalR();

            services.AddSwaggerGen();
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

                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}
