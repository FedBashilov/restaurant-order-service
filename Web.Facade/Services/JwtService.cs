// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using System.IdentityModel.Tokens.Jwt;

    public static class JwtService
    {
        public static string GetClaimValue(string jwtTokenEncoded, string claimType)
        {
            var accessToken = new JwtSecurityToken(jwtTokenEncoded);
            var userRole = accessToken.Claims.First(x => x.Type == claimType).Value;
            return userRole;
        }
    }
}
