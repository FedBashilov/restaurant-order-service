// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;
    using Web.Facade.Models.DTOs;
    using Web.Facade.Models.Responses;

    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetOrders(
            string accessToken,
            int offset = 0,
            int count = 100,
            bool orderDesc = false);

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
