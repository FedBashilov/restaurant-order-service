// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Extentions
{
    using Infrastructure.Menu.Interfaces;
    using Infrastructure.Menu.Services;
    using Infrastructure.Menu.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddMenuServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MenuServiceSettings>(configuration.GetSection("MenuServiceSettings"));
            services.AddHttpClient();
            services.TryAddSingleton<IHttpRequestFactory, HttpRequestFactory>();
            services.TryAddSingleton<IMenuService, MenuService>();
        }
    }
}
