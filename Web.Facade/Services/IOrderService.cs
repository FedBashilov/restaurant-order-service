// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetAllOrders(string accessToken);

        public Task<OrderResponse> GetOrder(int id, string accessToken);

        public Task<OrderResponse> CreateOrder(CreateOrderDto newOrder, string accessToken);

        public Task<OrderResponse> UpdateOrderStatus(int id, OrderStatus newStatus, string accessToken);
    }
}
