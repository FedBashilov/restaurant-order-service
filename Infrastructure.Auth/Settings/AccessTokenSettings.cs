// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth.Settings
{
    public record AccessTokenSettings
    {
        public string? SecretKey { get; init; }

        public string? Issuer { get; init; }

        public string? Audience { get; init; }
    }
}
