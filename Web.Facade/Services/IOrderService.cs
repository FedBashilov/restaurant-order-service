// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrders();

        public Task<Order> GetOrder(int id);

        public Task<Order> CreateOrder(Order newOrder);

        public Task<Order> UpdateOrderStatus(int id, OrderStatus newStatus);
    }
}
