// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.Responses
{
    public record OrderResponse
    {
        public int Id { get; init; }

        public ICollection<OrderMenuItemResponse>? MenuItems { get; init; }

        public string? ClientId { get; init; }

        public DateTime CreatedDate { get; init; }

        public DateTime CloseDate { get; init; }

        public string? Status { get; init; }

        public int TotalPrice { get; set; }

        public int CalculateTotalPrice()
        {
            this.TotalPrice = 0;

            if (this.MenuItems != null)
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
