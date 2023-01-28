// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class OrderResponse
    {
        public OrderResponse(Order order)
        {
            this.Id = order.Id;
            this.ClientId = order.ClientId;
            this.CreatedDate = order.CreatedDate;
            this.CloseDate = order.CloseDate;
            this.Status = order.Status;
            this.TotalPrice = 0;
            this.MenuItems = new List<OrderMenuItemResponse>();
        }

        public int Id { get; set; }

        public ICollection<OrderMenuItemResponse> MenuItems { get; set; }

        public string? ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CloseDate { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
