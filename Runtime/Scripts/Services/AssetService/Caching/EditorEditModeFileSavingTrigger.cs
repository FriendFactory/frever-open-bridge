namespace Bridge.Services.AssetService.Caching
{
    internal sealed class EditorEditModeFileSavingTrigger: SavingVersionFilesTrigger
    {
        public EditorEditModeFileSavingTrigger(AssetsCache assetsCache) : base(assetsCache)
        {
        }

        protected override void SubscribeToTriggerEvents()
        {
            AssetsCache.Updated += TriggerSaving;
        }

        protected override void UnsubscribeFromTriggerEvents()
        {
            AssetsCache.Updated -= TriggerSaving;
        }
    }
}