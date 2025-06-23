using UnityEngine;

namespace Bridge.EnvironmentCompatibility
{
    [CreateAssetMenu(menuName = "Friend Factory/Bridge/Bridge Version", order = 0, fileName = "Version")]
    public class BridgeVersion : ScriptableObject
    {
        public string Version;
    }
}
