// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Extentions
{
    using Infrastructure.Core.Models;

    public static class StringExtensions
    {
        public static OrderStatus ToOrderStatus(this string value)
        {
            return value switch
            {
                "InQueue" => OrderStatus.InQueue,
                "Ready" => OrderStatus.Ready,
                "Cooking" => OrderStatus.Cooking,
                "Closed" => OrderStatus.Closed,
                "Canceled" => OrderStatus.Canceled,
                _ => throw new ArgumentException("Invalid order status value"),
            };
        }
    }
}
