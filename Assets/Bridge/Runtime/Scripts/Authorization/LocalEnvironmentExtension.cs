namespace Bridge.Authorization
{
    /// <summary>
    /// Should be used for tracking traffic by Fiddler
    /// </summary>

#if USE_PROXY && UNITY_EDITOR

    public static class LocalEnvironmentExtension
    {
        public static string SetupAddressForLocalServer(this string url)
        {
            return url.Replace("localhost", "ipv4.fiddler");
        }
    }
    
#endif
}