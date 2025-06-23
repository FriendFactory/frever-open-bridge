using UnityEngine;

namespace Bridge.ExternalPackages.AsynAwaitUtility.Internal
{
    public class AsyncCoroutineRunner : MonoBehaviour
    {
        private static AsyncCoroutineRunner _instance;

        public static AsyncCoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Object.FindObjectOfType<AsyncCoroutineRunner>();
                }
                
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(AsyncCoroutineRunner)).AddComponent<AsyncCoroutineRunner>();
                }
                
                return _instance;
            }
        }

        private void Awake()
        {
            // Don't show in scene hierarchy
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            DontDestroyOnLoad(gameObject);
        }
    }
}
