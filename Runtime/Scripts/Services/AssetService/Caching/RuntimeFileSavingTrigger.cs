using UnityEngine;

namespace Bridge.Services.AssetService.Caching
{
    /// <summary>
    /// This one should be used for builds
    /// It saves version files data when user starts closing app
    /// </summary>
    internal sealed class RuntimeFileSavingTrigger: SavingVersionFilesTrigger
    {
        public RuntimeFileSavingTrigger(AssetsCache assetsCache) : base(assetsCache)
        {
        }

        protected override void SubscribeToTriggerEvents()
        {
            Application.focusChanged += OnAppFocusChanged;
        }

        protected override void UnsubscribeFromTriggerEvents()
        {
            Application.focusChanged -= OnAppFocusChanged;
        }
        
        private void OnAppFocusChanged(bool isFocused)
        {
            if(!isFocused)
                TriggerSaving();
        }
    }
}