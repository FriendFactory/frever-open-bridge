using System.Text.RegularExpressions;
using Bridge.EnvironmentCompatibility;
using UnityEditor;

[CustomEditor(typeof(BridgeVersion))]
public class VersionInspector : Editor
{
    private SerializedProperty _bridgeVersionField;
    private static readonly Regex VersionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)$");
    private static readonly Regex ApiVersionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)$");

    private void OnEnable()
    {
        _bridgeVersionField = serializedObject.FindProperty(nameof(BridgeVersion.Version));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_bridgeVersionField);

        var version = _bridgeVersionField.stringValue;
        var bridgeVersionMatch = VersionRegex.Match(version);
        if (!bridgeVersionMatch.Success)
        {
            EditorGUILayout.HelpBox("Bridge Version does not match 0.0.0 format", MessageType.Error);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}