// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    public interface IUserHubConnectionsRepository
    {
        public string GetConnectionId(string userId);

        public bool TryAddConnection(
            string userId,
            string connectionId);

        public bool TryRemoveConnection(string userId);
    }
}
