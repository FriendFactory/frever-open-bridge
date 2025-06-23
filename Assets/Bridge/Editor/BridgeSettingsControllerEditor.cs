using UnityEditor;
using UnityEngine;

namespace Bridge.Settings
{
    public sealed class BridgeSettingsControllerEditor: EditorWindow
    {
        [MenuItem("Tools/Friend Factory/Bridge/Settings")]
        public static void Display()
        {
            var window = GetWindow<BridgeSettingsControllerEditor>();
            window.name = "Bridge Settings";
        }

        private const string USE_PROXY_PREDEFINED_SYMBOL = "USE_PROXY";
        
        private BridgeSettings _bridgeSettings;

        private string BridgeSettingsFileName => nameof(BridgeSettings);
        
        private void OnEnable()
        {
            _bridgeSettings = LoadSettings();
            if (_bridgeSettings == null)
            {
                CreateSettings();
            }

            _bridgeSettings = LoadSettings();
            EditorUtility.SetDirty(_bridgeSettings);
        }

        private void OnGUI()
        {
            if (_bridgeSettings == null)
            {
                EditorGUILayout.HelpBox("Settings is null", MessageType.Error);
                return;
            }
            
            EditorGUI.BeginChangeCheck();

            _bridgeSettings.TlsSecurity = EditorGUILayout.Toggle("TLS Security", _bridgeSettings.TlsSecurity);
            _bridgeSettings.UseProtobuf = EditorGUILayout.Toggle("Use Protobuf", _bridgeSettings.UseProtobuf);

            EditorGUILayout.LabelField("Proxy settings:", EditorStyles.boldLabel);

            var val = EditorGUILayout.Toggle("Enabled:",_bridgeSettings.UseProxy);
            if (val != _bridgeSettings.UseProxy)
            {
                _bridgeSettings.UseProxy = val;
                UpdatePredefinedSymbols(val);
            }

            GUI.enabled = val;
            
            _bridgeSettings.ProxyIP = EditorGUILayout.TextField("IP:",_bridgeSettings.ProxyIP);
            _bridgeSettings.ProxyToolPort = (ProxyToolPort)EditorGUILayout.EnumPopup("ProxyTool", _bridgeSettings.ProxyToolPort);
            
            EditorGUILayout.LabelField("Port", _bridgeSettings.ProxyPort.ToString());
            
            GUI.enabled = true;
            
            if (EditorGUI.EndChangeCheck())
            {
                AssetDatabase.SaveAssets();
            }
        }

        private void UpdatePredefinedSymbols(bool useProxy)
        {
            var definedSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            if (useProxy)
            {
                definedSymbols += $";{USE_PROXY_PREDEFINED_SYMBOL}";
            }
            else
            {
                definedSymbols = definedSymbols
                    .Replace(USE_PROXY_PREDEFINED_SYMBOL, "");
            }
            
            definedSymbols = definedSymbols.Replace(";;", ";");
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, definedSymbols);
        }

        private BridgeSettings LoadSettings()
        {
            return Resources.Load(BridgeSettingsFileName, typeof(BridgeSettings)) as BridgeSettings;
        }

        private void CreateSettings()
        {
            var bridgeSettings = CreateInstance<BridgeSettings>();

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            
            AssetDatabase.CreateAsset(bridgeSettings, $"Assets/Resources/{BridgeSettingsFileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}