// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record FbTokenDTO
    {
        [Required(ErrorMessage = "The FirebaseToken param is required")]
        [MinLength(1, ErrorMessage = "The FirebaseToken param must have at least 1 item")]
        public string? FirebaseToken { get; init; }
    }
}
