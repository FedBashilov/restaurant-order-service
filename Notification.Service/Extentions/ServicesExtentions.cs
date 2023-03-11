// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Extentions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Notifications.Service;

    public static class ServicesExtentions
    {
        public static void AddNotificationServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IUserHubConnectionsRepository, ClientHubConnectionsRepository>();
        }
    }
}
