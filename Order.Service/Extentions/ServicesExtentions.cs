// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Orders.Service.Extentions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orders.Service;
    using Orders.Service.Interfaces;

    public static class ServicesExtentions
    {
        public static void AddOrderServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IOrderService, OrderService>();
        }
    }
}
