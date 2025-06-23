using UnityEditor;
using UnityEngine;

namespace Bridge.Settings
{
    [CustomEditor(typeof(BridgeSettings))]
    public sealed class BridgeSettingsEditor : Editor
    {
        private BridgeSettings _bridgeSettings;

        private void OnEnable()
        {
            _bridgeSettings = serializedObject.targetObject as BridgeSettings;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.Toggle(nameof(_bridgeSettings.TlsSecurity), _bridgeSettings.TlsSecurity);
            EditorGUILayout.Toggle(nameof(_bridgeSettings.UseProtobuf), _bridgeSettings.UseProtobuf);
            EditorGUILayout.Toggle(nameof(_bridgeSettings.UseProxy), _bridgeSettings.UseProxy);
            EditorGUILayout.TextField(nameof(_bridgeSettings.ProxyIP), _bridgeSettings.ProxyIP);
            EditorGUILayout.EnumPopup("ProxyTool",_bridgeSettings.ProxyToolPort);
            EditorGUILayout.TextField("Port", _bridgeSettings.ProxyPort.ToString());
            GUI.enabled = true;
            
            EditorGUILayout.HelpBox("Settings are readonly from inspector. You can modify them via Tools/Bridge/Settings", 
                MessageType.Info);
        }
    }
}
