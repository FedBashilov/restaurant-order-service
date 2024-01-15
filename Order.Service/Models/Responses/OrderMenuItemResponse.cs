// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.Responses
{
    public record OrderMenuItemResponse
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? ImageUrl { get; init; }

        public int Price { get; init; }

        public int Amount { get; init; }
    }
}
