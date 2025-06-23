using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bridge
{
    public class Utils: Editor
    {
        [MenuItem("Tools/Friend Factory/Bridge/Clear Cache")]
        public static void ClearCache()
        {
            var bridge = new ServerBridge();
            var res =  bridge.ClearCacheAsync().Result;
            if(res.IsSuccess)
                Debug.Log("Cache cleaning succeed");
            else
                Debug.LogError(res.ErrorMessage);
        }
        
        [MenuItem("Tools/Friend Factory/Bridge/Clear Auth Data")]
        public static void ClearAuthData()
        {
            var path = $"{Application.persistentDataPath}/{Constants.FileNameConstants.AUTH_FILE_NAME}";
            if (File.Exists(path))
            {
                Debug.Log("Auth data has been deleted successfully");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Auth data file does not exist");
            }
        }
    }
}
