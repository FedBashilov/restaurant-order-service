// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Infrastructure.Menu.Services;
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;
    using Web.Facade.Models.DTOs;
    using Web.Facade.Models.Responses;

    public class OrderService : IOrderService
    {
        private readonly IMenuService menuService;
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;

        public OrderService(IMenuService menuService, IDbContextFactory<OrderDatabaseContext> dbCxtFactory)
        {
            this.menuService = menuService;
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<IEnumerable<OrderResponse>> GetOrders(
            string accessToken,
            int offset = 0,
            int count = 100,
            bool orderDesc = false)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var orderQuery = orderDesc ?
                dbContext.Orders.OrderByDescending(x => x.Id) :
                dbContext.Orders.OrderBy(x => x.Id);

            var pageQuery = orderQuery.Skip(offset).Take(count);

            var orders = await pageQuery.ToListAsync();

            var ordersResponse = new List<OrderResponse>();

            foreach (var order in orders)
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

        public async Task<OrderResponse> CreateOrder(CreateOrderDTO orderDto, string clientId, string accessToken)
        {
            var newOrder = new Order()
            {
                Status = orderDto.Status,
                ClientId = clientId,
                CreatedDate = DateTime.UtcNow,
            };

            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = dbContext.Orders.Add(newOrder).Entity;

            if (orderDto.MenuItems != null)
            {
                foreach (var menuItem in orderDto.MenuItems)
                {
                    if (!await this.IsMenuItemExist(menuItem.MenuItemId, accessToken))
                    {
                        throw new NotFoundException($"Menu item with id = {menuItem.MenuItemId} not found");
                    }
                }

                await dbContext.SaveChangesAsync();

                foreach (var menuItem in orderDto.MenuItems)
                {
                    await dbContext.OrderMenuItems.AddAsync(new OrderMenuItem(menuItem) { OrderId = order.Id });
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
 
            if (newStatus == OrderStatus.Finished || newStatus == OrderStatus.Canceled)
            {
                order.CloseDate = DateTime.UtcNow;
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

        private async Task<bool> IsMenuItemExist(int menuItemId, string accessToken)
        {
            var menuItem = await this.menuService.GetMenuItem(menuItemId, accessToken);
            return menuItem != null;
        }
    }
}
