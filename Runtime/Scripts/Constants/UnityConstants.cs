using UnityEngine;

namespace Bridge.Constants
{
    /// <summary>
    /// Required for getting unity data from non main thread
    /// </summary>
    internal static class UnityConstants
    {
        /// <summary>
        /// Initialize variable in main thread
        /// </summary>
        public static void Init()
        {
            var persistentDataPath = PersistentDataPath;
            var unityVersion = UnityVersion;
        }
        
        private static string _persistentDataPath;
        public static string PersistentDataPath => _persistentDataPath ??= Application.persistentDataPath;

        private static string _unityVersion;
        public static string UnityVersion => _unityVersion ??= Application.unityVersion;
    }
}
