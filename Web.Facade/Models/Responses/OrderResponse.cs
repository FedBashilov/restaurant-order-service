// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.Responses
{
    using Microsoft.OpenApi.Extensions;

    public class OrderResponse
    {
        public OrderResponse(Order order)
        {
            this.Id = order.Id;
            this.ClientId = order.ClientId;
            this.CreatedDate = order.CreatedDate;
            this.CloseDate = order.CloseDate;
            this.Status = order.Status.GetDisplayName();
            this.TotalPrice = 0;
            this.MenuItems = new List<OrderMenuItemResponse>();
        }

        public int Id { get; set; }

        public ICollection<OrderMenuItemResponse> MenuItems { get; set; }

        public string? ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CloseDate { get; set; }

        public string Status { get; set; }

        public int TotalPrice { get; set; }
    }
}
