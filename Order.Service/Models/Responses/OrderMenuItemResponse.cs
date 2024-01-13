// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.Responses
{
    public class OrderMenuItemResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public int Price { get; set; }

        public int Amount { get; set; }
    }
}
