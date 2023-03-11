// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database.Extentions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesExtentions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<OrderDatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
