// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service
{
    public interface IUserHubConnectionsRepository
    {
        public IEnumerable<string> GetConnectionIds(string userId);

        public bool TryAddConnection(
            string connectionId,
            string userId);

        public bool TryRemoveConnection(string connectionId);
    }
}
