﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Infrastructure.Menu.Services;
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;

    public class OrderService : IOrderService
    {
        private readonly IMenuService menuService;
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;

        public OrderService(IMenuService menuService, IDbContextFactory<OrderDatabaseContext> dbCxtFactory)
        {
            this.menuService = menuService;
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrders(string accessToken)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var ordersResponse = new List<OrderResponse>();

            foreach (var order in dbContext.Orders)
            {
                var orderResponse = await this.GetOrderResponse(order, dbContext, accessToken);

                ordersResponse.Add(orderResponse);
            }

            return ordersResponse;
        }

        public async Task<OrderResponse> GetOrder(int id, string accessToken)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Not found order with id = {id}");
            }

            var orderResponse = await this.GetOrderResponse(order, dbContext, accessToken);

            return orderResponse;
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderDto orderDto, string accessToken)
        {
            var newOrder = new Order(orderDto);

            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = dbContext.Orders.Add(newOrder).Entity;

            await dbContext.SaveChangesAsync();

            if (orderDto.MenuItemIds != null)
            {
                foreach (var menuItem in orderDto.MenuItemIds)
                {
                    await dbContext.OrderMenuItems.AddAsync(new OrderMenuItem(menuItem) { OrderId = order.Id});
                }
            }

            await dbContext.SaveChangesAsync();

            var orderResponse = await this.GetOrderResponse(order, dbContext, accessToken);

            order.TotalPrice = orderResponse.TotalPrice;

            await dbContext.SaveChangesAsync();

            return orderResponse;
        }

        public async Task<OrderResponse> UpdateOrderStatus(int id, OrderStatus newStatus, string accessToken)
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

            var orderResponse = await this.GetOrderResponse(order, dbContext, accessToken);

            return orderResponse;
        }

        private async Task<OrderResponse> GetOrderResponse(Order order, OrderDatabaseContext dbContext, string accessToken)
        {
            var orderResponse = new OrderResponse(order);
            var orderMenuItems = dbContext.OrderMenuItems.Where(omi => omi.OrderId == order.Id);
            foreach (var orderMenuItem in orderMenuItems)
            {
                var menuItem = await this.menuService.GetMenuItem(orderMenuItem.MenuItemId, accessToken);

                orderResponse.MenuItems.Add(new OrderMenuItemResponse(menuItem, orderMenuItem.Amount));

                orderResponse.TotalPrice += menuItem.Price * orderMenuItem.Amount;
            }

            return orderResponse;
        }
    }
}
