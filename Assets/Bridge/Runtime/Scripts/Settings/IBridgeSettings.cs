namespace Bridge.Settings
{
    public interface IBridgeSettings
    { 
        bool TlsSecurity { get; }
        bool UseProtobuf { get; }
        bool UseProxy { get; }
        string ProxyIP { get; }
        int ProxyPort { get; }
    }
}