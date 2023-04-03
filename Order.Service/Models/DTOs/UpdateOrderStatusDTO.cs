// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "The Status param is required")]
        [NotNull]
        public string? Status { get; set; }
    }
}
