// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth.Constants
{
    public static class ClaimTypes
    {
        /// <summary>
        /// A claim that contains the user role.
        /// </summary>
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// A claim that contains the user identifier.
        /// </summary>
        public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";
    }
}
