// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    public interface IFbNotificationService
    {
        public Task SendMessage(string clientId, string payload, string eventType);
    }
}