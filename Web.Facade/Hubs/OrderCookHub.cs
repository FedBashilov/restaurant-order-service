// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Models;
    using Web.Facade.Models.Responses;

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
            await using var dbContext = this.dbCxtFactory.CreateDbContext();

            var activeOrders = dbContext.Orders.Where(o =>
                o.Status != OrderStatus.Finished &&
                o.Status != OrderStatus.Canceled).ToList();


            var activeOrderResponse = new List<OrderResponse>();
            foreach (var order in activeOrders)
            {
                activeOrderResponse.Add(new OrderResponse(order));
            }

            await this.Clients.Client(this.Context.ConnectionId).SendAsync("Notify", activeOrderResponse);

            await base.OnConnectedAsync();
        }
    }
}
