using UnityEngine;

namespace Bridge.Settings
{
    internal sealed class BridgeSettings: ScriptableObject, IBridgeSettings
    {
        public bool TlsSecurity
        {
            get => _tlsSecurity;
            set
            {
                _tlsSecurity = value;
                if (value) UseProxy = false;
            }
        }

        public bool UseProtobuf
        {
            get => _useProtobuf;
            set => _useProtobuf = value;
        }
        
        public bool UseProxy
        {
            get => _useProxy;
            set
            {
                _useProxy = value;
                if (value) TlsSecurity = false;
            }
        }

        public string ProxyIP
        {
            get => _proxyIp;
            set => _proxyIp = value;
        }

        public int ProxyPort => (int)_proxyToolPort;

        public ProxyToolPort ProxyToolPort
        {
            get => _proxyToolPort;
            set => _proxyToolPort = value;
        }

        [SerializeField] private bool _tlsSecurity;
        [SerializeField] private bool _useProtobuf;
        [SerializeField] private bool _useProxy;
        [SerializeField] private string _proxyIp;
        [SerializeField] private ProxyToolPort _proxyToolPort = ProxyToolPort.Fiddler;
    }

    internal enum ProxyToolPort
    {
        Fiddler = 8888,
        HttpToolKit = 8000
    }
}