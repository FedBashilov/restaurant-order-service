// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service.Hubs
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Responses;
    using Infrastructure.Database;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "cook")]
    public class OrderCookHub : Hub
    {
        private readonly IDbContextFactory<OrderDatabaseContext> dbCxtFactory;

        public OrderCookHub(IDbContextFactory<OrderDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public override async Task OnConnectedAsync()
        {
            await using var dbContext = dbCxtFactory.CreateDbContext();

            var activeOrders = dbContext.Orders.Where(o =>
                o.Status != OrderStatus.Closed &&
                o.Status != OrderStatus.Canceled).ToList();


            var activeOrderResponse = new List<OrderResponse>();
            foreach (var order in activeOrders)
            {
                activeOrderResponse.Add(new OrderResponse()
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    CreatedDate = order.CreatedDate,
                    CloseDate = order.CloseDate,
                    Status = order.Status.ToString(),
                    TotalPrice = order.TotalPrice,
                    MenuItems = new List<OrderMenuItemResponse>()
                });
            }

            await this.Clients.Client(this.Context.ConnectionId).SendAsync("Connect", activeOrderResponse);

            await base.OnConnectedAsync();
        }
    }
}
