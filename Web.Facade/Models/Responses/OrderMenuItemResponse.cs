// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models.Responses
{
    using Infrastructure.Menu.Models;

    public class OrderMenuItemResponse
    {
        public OrderMenuItemResponse(MenuItem menuItem, decimal amount)
        {
            this.Id = menuItem.Id;
            this.Name = menuItem.Name;
            this.Price = menuItem.Price;
            this.Amount = amount;
        }

        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}
