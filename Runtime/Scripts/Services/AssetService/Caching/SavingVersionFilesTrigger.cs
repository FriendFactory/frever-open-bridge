namespace Bridge.Services.AssetService.Caching
{
    internal abstract class SavingVersionFilesTrigger
    {
        protected readonly AssetsCache AssetsCache;
        private bool _running;
        
        protected SavingVersionFilesTrigger(AssetsCache assetsCache)
        {
            AssetsCache = assetsCache;
        }
        
        protected void TriggerSaving()
        {
            AssetsCache.SaveCacheDataToFile();
        }

        public void Run()
        {
            if(_running) return;
            
            SubscribeToTriggerEvents();
            _running = true;
        }

        public void Stop()
        {
            if(!_running) return;
            
            UnsubscribeFromTriggerEvents();
            _running = false;
        }
        
        protected abstract void SubscribeToTriggerEvents();
        
        protected abstract void UnsubscribeFromTriggerEvents();
    }
}
