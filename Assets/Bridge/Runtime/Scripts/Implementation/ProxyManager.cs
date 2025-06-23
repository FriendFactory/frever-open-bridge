using System;
using System.Net;
using System.Net.Http;
using BestHTTP;
using Bridge.Settings;

namespace Bridge
{
    internal sealed class ProxyManager
    {
        private readonly IBridgeSettings _bridgeSettings;

        public ProxyManager(IBridgeSettings bridgeSettings)
        {
            _bridgeSettings = bridgeSettings;
        }

        public bool ProxyEnabled => _bridgeSettings.UseProxy;
        
        public void SetupGlobalForBestHttpRequests()
        {
            if(!ProxyEnabled) return;

#if UNITY_EDITOR
            HTTPManager.Proxy = new HTTPProxy(new Uri($"http://localhost:{_bridgeSettings.ProxyPort}"));
#elif UNITY_IOS
            HTTPManager.Proxy = new HTTPProxy(new Uri($"http://{_bridgeSettings.ProxyIP}:{_bridgeSettings.ProxyPort}"));
#endif
        }

        public void SetupProxy(HttpClientHandler target)
        {
            if(!ProxyEnabled) return;
            
#if UNITY_EDITOR
            target.Proxy = new WebProxy("localhost", _bridgeSettings.ProxyPort);
#elif UNITY_IOS
            target.Proxy = new WebProxy(_bridgeSettings.ProxyIP, _bridgeSettings.ProxyPort);
#endif
        }
    }
}