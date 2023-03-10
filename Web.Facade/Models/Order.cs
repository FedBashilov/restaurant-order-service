// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string? ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CloseDate { get; set; }

        public OrderStatus Status { get; set; }

        public int TotalPrice { get; set; }
    }
}
