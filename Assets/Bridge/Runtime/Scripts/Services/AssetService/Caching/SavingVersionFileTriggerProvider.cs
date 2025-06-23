using UnityEngine;

namespace Bridge.Services.AssetService.Caching
{
    /// <summary>
    /// Provides different triggers for saving cache meta data file
    /// Depends on mode - build vs editor play mode vs editor edit mode
    /// </summary>
    internal class SavingVersionFileTriggerProvider
    {
        private readonly AssetsCache _assetsCache;

        public SavingVersionFileTriggerProvider(AssetsCache assetsCache)
        {
            _assetsCache = assetsCache;
        }

        public SavingVersionFilesTrigger GetTrigger()
        {
#if !UNITY_EDITOR
            return new RuntimeFileSavingTrigger(_assetsCache);
#else
            if (Application.isPlaying)
                return new EditorPlayModeSavingTrigger(_assetsCache);
            else
                return new EditorEditModeFileSavingTrigger(_assetsCache);
#endif
        }
    }
}