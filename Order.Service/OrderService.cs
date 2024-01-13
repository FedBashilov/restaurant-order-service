// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Orders.Service
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.DTOs;
    using Infrastructure.Core.Models.Responses;
    using Infrastructure.Database;
    using Infrastructure.Menu.Models;
    using Infrastructure.Menu.Services;
    using Microsoft.EntityFrameworkCore;
    using Orders.Service.Exceptions;

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
            string? accessToken,
            int offset = 0,
            int count = 100,
            bool orderDesc = false,
            bool onlyActive = false,
            string? clientId = null)
        {
            await using var dbContext = this.dbCxtFactory.CreateDbContext();

            var activeQuery = onlyActive ?
                dbContext.Orders.Where(o => o.Status != OrderStatus.Closed && o.Status != OrderStatus.Canceled) :
                dbContext.Orders;

            var clientQuery = clientId != null ?
                activeQuery.Where(o => o.ClientId == clientId) :
                activeQuery;

            var orderQuery = orderDesc ?
                clientQuery.OrderByDescending(x => x.Id) :
                clientQuery.OrderBy(x => x.Id);

            var pageQuery = orderQuery.Skip(offset).Take(count);

            var orders = await pageQuery.ToListAsync();

            var orderTasks = new List<Task<OrderResponse>>();

            foreach (var order in orders)
            {
                orderTasks.Add(this.CreateOrderResponse(order, dbContext, accessToken));
            }

            var ordersResponse = await Task.WhenAll(orderTasks);

            return ordersResponse;
        }

        public async Task<OrderResponse> GetOrder(int id, string? accessToken)
        {
            await using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Not found order with id = {id}");
            }

            var orderResponse = await this.CreateOrderResponse(order, dbContext, accessToken);

            return orderResponse;
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderDTO orderDto, string clientId, string? accessToken)
        {
            var newOrder = new Order()
            {
                Status = OrderStatus.InQueue,
                ClientId = clientId,
                CreatedDate = DateTime.UtcNow,
            };

            await using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = dbContext.Orders.Add(newOrder).Entity;

            if (orderDto.MenuItems != null)
            {
                foreach (var menuItem in orderDto.MenuItems)
                {
                    if (!await this.menuService.IsMenuItemExist(menuItem.MenuItemId, accessToken))
                    {
                        throw new NotFoundException($"Menu item with id = {menuItem.MenuItemId} not found");
                    }
                }

                await dbContext.SaveChangesAsync();

                foreach (var menuItem in orderDto.MenuItems)
                {
                    await dbContext.OrderMenuItems.AddAsync(new OrderMenuItem()
                    {
                        MenuItemId = menuItem.MenuItemId,
                        Amount = menuItem.Amount,
                        OrderId = order.Id,
                    });
                }
            }

            await dbContext.SaveChangesAsync();

            var orderResponse = await this.CreateOrderResponse(order, dbContext, accessToken);

            order.TotalPrice = orderResponse.TotalPrice;

            await dbContext.SaveChangesAsync();

            return orderResponse;
        }

        public async Task<OrderResponse> UpdateOrderStatus(int id, OrderStatus newStatus, string? accessToken)
        {
            await using var dbContext = this.dbCxtFactory.CreateDbContext();
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Not found menu item with id = {id}");
            }

            if (newStatus == OrderStatus.Closed || newStatus == OrderStatus.Canceled)
            {
                order.CloseDate = DateTime.UtcNow;
            }

            order.Status = newStatus;

            var newOrder = dbContext.Orders.Update(order).Entity;
            await dbContext.SaveChangesAsync();

            var orderResponse = await this.CreateOrderResponse(order, dbContext, accessToken);

            return orderResponse;
        }

        private async Task<OrderResponse> CreateOrderResponse(Order order, OrderDatabaseContext dbContext, string? accessToken)
        {
            var orderResponse = new OrderResponse()
            {
                Id = order.Id,
                ClientId = order.ClientId,
                CreatedDate = order.CreatedDate,
                CloseDate = order.CloseDate,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                MenuItems = new List<OrderMenuItemResponse>(),
            };

            var menuItemTasks = new List<Task<MenuItem>>();

            var orderMenuItems = dbContext.OrderMenuItems.Where(omi => omi.OrderId == order.Id).ToList();
            foreach (var orderMenuItem in orderMenuItems)
            {
                menuItemTasks.Add(this.menuService.GetMenuItem(orderMenuItem.MenuItemId, accessToken));
            }

            var menuItems = await Task.WhenAll(menuItemTasks);

            for (var i = 0; i < menuItems.Length; i++)
            {
                orderResponse.MenuItems.Add(new OrderMenuItemResponse()
                {
                    Id = menuItems[i].Id,
                    Name = menuItems[i].Name,
                    ImageUrl = menuItems[i].ImageUrl,
                    Price = menuItems[i].Price,
                    Amount = orderMenuItems[i].Amount,
                });
            }

            orderResponse.CalculateTotalPrice();

            return orderResponse;
        }
    }
}
