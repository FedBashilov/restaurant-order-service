// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
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
