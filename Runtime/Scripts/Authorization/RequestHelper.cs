using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BestHTTP;
using Bridge.Authorization.HttpExtensions;
using Bridge.Constants;
using Bridge.Settings;

namespace Bridge.Authorization
{
    internal sealed class RequestHelper: IRequestHelper
    {
        private const int REQUEST_RETRIES_COUNT = 3;
        private const string SessionHeader = "X-Session-Id";
        private const string UnityVersionHeader = "x-unity-version";
        private readonly ProxyManager _proxyManager;
        private readonly string _sessionId;
        private readonly IBridgeSettings _bridgeSettings;

        private AuthToken _token;

        public RequestHelper(ProxyManager proxyManager, string sessionId, IBridgeSettings bridgeSettings)
        {
            _proxyManager = proxyManager;
            _sessionId = sessionId;
            _bridgeSettings = bridgeSettings;
        }

        public AuthToken Token => _token;

        public void SetToken(AuthToken token)
        {
            _token = token;
        }
        
        public HTTPRequest CreateRequest(Uri uri, HTTPMethods method, bool addToken, bool protobuf)
        {
            var request = new HTTPRequest(uri, method);
            if (addToken) AddToken(request);
            if (protobuf) SetupProtobufIfEnabled(request);
            request.MaxRetries = REQUEST_RETRIES_COUNT;
            request.AddHeader(SessionHeader, _sessionId);
            request.AddHeader(UnityVersionHeader, UnityConstants.UnityVersion);
            return request;
        }

        public HTTPRequest CreateRequest(string url, HTTPMethods method, bool addToken, bool protobuf)
        {
            return CreateRequest(new Uri(url), method, addToken, protobuf);
        }

        public HTTPRequest CreateRequest(string url, HTTPMethods method, string token)
        {
            var request = new HTTPRequest(new Uri(url), method);
            AddToken(request, token);
            request.MaxRetries = REQUEST_RETRIES_COUNT;
            request.AddHeader(SessionHeader, _sessionId);
            request.AddHeader(UnityVersionHeader, UnityConstants.UnityVersion);
            return request;
        }

        public HttpClient CreateClient(bool addToken, bool addRetryPolicy = true)
        {
            var handler = new HttpClientHandler();
            if (_proxyManager.ProxyEnabled) _proxyManager.SetupProxy(handler);
            var client = addRetryPolicy
                ? new HttpClient(new RetryHandler(handler)) 
                : new HttpClient(handler);
            if (addToken) AddToken(client);
            client.DefaultRequestHeaders.Add(SessionHeader, _sessionId);
            return client;
        }
        
        private void AddToken(HttpClient target)
        {
            if (_token == null) return;
            target.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Value);
        }
        
        private void AddToken(HTTPRequest target)
        {
            if (Token == null) return;
            var token = Token.Value;
            AddToken(target, token);
        }

        private static void AddToken(HTTPRequest target, string token)
        {
            target.AddHeader("Authorization", $"Bearer {token}");
        }

        public void SetupProtobufIfEnabled(HTTPRequest target)
        {
            if (!_bridgeSettings.UseProtobuf) return;
            target.AddHeader("Accept", "application/vnd.google.protobuf");
        }
    }
}