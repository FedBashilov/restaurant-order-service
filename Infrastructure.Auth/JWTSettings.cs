// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth
{
    public class JWTSettings
    {
        public string? SecretKey { get; set; }

        public string? Issuer { get; set; }

        public string? Audience { get; set; }
    }
}
