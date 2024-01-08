// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public ICollection<OrderMenuItemResponse>? MenuItems { get; set; }

        public string? ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CloseDate { get; set; }

        public string? Status { get; set; }

        public int TotalPrice { get; set; }

        public int CalculateTotalPrice()
        {
            if (this.TotalPrice == 0 && this.MenuItems != null)
            {
                foreach (var menuItem in this.MenuItems)
                {
                    this.TotalPrice += menuItem.Price * menuItem.Amount;
                }
            }

            return this.TotalPrice;
        }
    }
}
