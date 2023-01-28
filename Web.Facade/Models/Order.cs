// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class Order
    {
        public Order()
        {
        }

        public Order(CreateOrderDto orderDto)
        {
            this.ClientId = orderDto.ClientId;
            this.Status = orderDto.Status;
        }

        public int Id { get; set; }

        public string? ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CloseDate { get; set; }

        public OrderStatus Status { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }
    }
}
