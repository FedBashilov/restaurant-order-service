// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record CreateOrderMenuItemDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "The MenuItemId param must be positive")]
        public int MenuItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Amount param must be greater than 1")]
        public int Amount { get; set; }
    }
}
