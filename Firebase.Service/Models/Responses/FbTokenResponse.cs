// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service.Models.Responses
{
    public record FbTokenResponse
    {
        public string? ClientId { get; init; }

        public string? FirebaseToken { get; init; }
    }
}
