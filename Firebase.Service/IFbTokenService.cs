// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    public interface IFbTokenService
    {
        public Task SetFbToken(string clientId, string fbToken);

        public Task<string> GetFbToken(string clientId);
    }
}