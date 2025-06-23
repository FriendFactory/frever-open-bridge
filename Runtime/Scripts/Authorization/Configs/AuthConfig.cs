namespace Bridge.Authorization.Configs
{
    [System.Serializable]
    internal sealed class AuthConfig
    {
        public readonly string Host;
        public readonly string IdentityServerURL;
        public readonly FFEnvironment Environment; 

        public AuthConfig(string host, FFEnvironment environment)
        {
            Host = host;
            IdentityServerURL = Extensions.CombineUrls(Host, "auth");
            Environment = environment;
        }
    }
}