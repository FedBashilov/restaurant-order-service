// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;

    public class OrderService : IOrderService
    {
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;

        public OrderService(IDbContextFactory<OrderDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            return await Task.FromResult(dbContext.Orders.ToList());
        }

        public async Task<Order> GetOrder(int id)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Not found order with id = {id} while executing GetOrder method");
            }

            return order;
        }

        public async Task<Order> CreateOrder(Order newOrder)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = dbContext.Orders.Add(newOrder).Entity;
            await dbContext.SaveChangesAsync();

            return order;
        }

        public async Task<Order> UpdateOrderStatus(int id, OrderStatus newStatus)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Not found menu item with id = {id}");
            }

            order.Status = newStatus;

            var newOrder = dbContext.Orders.Update(order).Entity;

            await dbContext.SaveChangesAsync();

            return newOrder;
        }
    }
}
