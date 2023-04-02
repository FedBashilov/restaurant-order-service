// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Menu.Extentions
{
    using Infrastructure.Menu.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServicesExtentions
    {
        public static void AddMenuServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MenuConfig>(configuration.GetSection("MenuService"));
            services.AddHttpClient();
            services.TryAddSingleton<IHttpRequestFactory, HttpRequestFactory>();
            services.TryAddSingleton<IMenuService, MenuService>();
        }
    }
}
