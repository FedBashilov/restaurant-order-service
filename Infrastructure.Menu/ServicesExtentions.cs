// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu
{
    using Infrastructure.Menu.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddMenuServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.TryAddSingleton<IHttpRequestFactory, HttpRequestFactory>();
            services.TryAddSingleton<IMenuService, MenuService>();
        }
    }
}
