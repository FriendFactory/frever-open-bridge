namespace Bridge.Settings
{
    internal sealed class DefaultSettings: IBridgeSettings
    {
        public bool TlsSecurity => false;
        public bool UseProtobuf => false;
        public bool UseProxy => false;
        public string ProxyIP => string.Empty;
        public int ProxyPort => -1;
    }
}