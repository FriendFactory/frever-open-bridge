using System;

namespace Bridge.Authorization
{
    [Serializable]
    internal class TokenResponse
    {
        public string access_token;
        public string refresh_token;
        public string server_url;
        public string asset_server;
        public string transcoding_server;
        public string video_server;
        public string social_server;
        public string notification_server;
        public string client_server;
        public string chat_server;
        public string assetmanager_server;
    }
}