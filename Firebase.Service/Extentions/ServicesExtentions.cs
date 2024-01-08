// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service.Extentions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddFirebaseServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IFbNotificationService, FbNotificationService>();
            services.TryAddSingleton<IFbTokenService, FbTokenService>();
        }
    }
}
