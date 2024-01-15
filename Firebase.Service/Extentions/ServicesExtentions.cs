// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service.Extentions
{
    using Firebase.Service.Interfaces;
    using Firebase.Service.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddFirebaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.Configure<FirebaseSettings>(configuration.GetSection("FirebaseSettings"));
            services.TryAddSingleton<IFbNotificationService, FbNotificationService>();
            services.TryAddSingleton<IFbTokenService, FbTokenService>();
        }
    }
}
