// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Orders.Service
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.DTOs;
    using Infrastructure.Core.Models.Responses;

    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetOrders(
            string accessToken,
            int offset = 0,
            int count = 100,
            bool orderDesc = false,
            string? clientId = null);

        public Task<OrderResponse> GetOrder(
            int id,
            string accessToken);

        public Task<OrderResponse> CreateOrder(
            CreateOrderDTO newOrder,
            string clientId,
            string accessToken);

        public Task<OrderResponse> UpdateOrderStatus(
            int id,
            OrderStatus newStatus,
            string accessToken);
    }
}
