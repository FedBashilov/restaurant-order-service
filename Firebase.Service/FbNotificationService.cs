// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Firebase.Service
{
    using Firebase.Service.Interfaces;
    using Firebase.Service.Settings;
    using FirebaseAdmin;
    using FirebaseAdmin.Messaging;
    using Google.Apis.Auth.OAuth2;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class FbNotificationService : IFbNotificationService
    {
        private readonly IFbTokenService fbTokenService;
        private readonly FirebaseSettings fbSettings;

        private readonly ILogger<FbNotificationService> logger;

        public FbNotificationService(
            IFbTokenService fbTokenService,
            IOptions<FirebaseSettings> fbSettings,
            ILogger<FbNotificationService> logger)
        {
            this.fbSettings = fbSettings.Value;
            this.fbTokenService = fbTokenService;
            this.logger = logger;

            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault(),
                    ProjectId = this.fbSettings.ProjectId,
                });
            }
            catch (Exception e)
            {
                this.logger.LogError("Firebase initialization failed!" + e.Message);
            }
        }

        public async Task SendMessage(string eventType, string clientId, string payload)
        {
            try
            {
                var token = await this.fbTokenService.GetFbToken(clientId);

                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "payload", payload },
                        { "event", eventType },
                    },
                    Token = token,
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                this.logger.LogInformation("Sent message to Firebase: " + response, payload);
            }
            catch
            {
                this.logger.LogError($"Firebase message sending failed! eventType={eventType}, clientId={clientId}, payload={payload}");
            }
        }
    }
}