// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Notifications.Service
{
    public class ClientHubConnectionsRepository : IUserHubConnectionsRepository
    {
        private readonly Dictionary<string, string> connections = new();

        public bool TryAddConnection(string connectionId, string userId)
        {
            return connections.TryAdd(connectionId, userId);
        }

        public bool TryRemoveConnection(string connectionId)
        {
            return connections.Remove(connectionId, out _);
        }

        public IEnumerable<string> GetConnectionIds(string userId)
        {
            return connections.Where(x => x.Value == userId).Select(x => x.Key);
        }
    }
}
