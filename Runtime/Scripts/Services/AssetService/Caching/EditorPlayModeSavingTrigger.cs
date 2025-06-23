#if UNITY_EDITOR

using UnityEditor;

namespace Bridge.Services.AssetService.Caching
{
    /// <summary>
    /// This class should be available only in Unity Editor.
    /// It saves version files when you stop running app inside Unity Editor
    /// </summary>
    internal sealed class EditorPlayModeSavingTrigger: SavingVersionFilesTrigger
    {
        public EditorPlayModeSavingTrigger(AssetsCache assetsCache) : base(assetsCache)
        {
        }

        protected override void SubscribeToTriggerEvents()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        protected override void UnsubscribeFromTriggerEvents()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnPlayModeChanged(PlayModeStateChange playMode)
        {
            if(playMode == PlayModeStateChange.ExitingPlayMode)
                TriggerSaving();
        }
    }
}

#endif