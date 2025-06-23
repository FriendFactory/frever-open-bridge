using System;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    /// <summary>
    /// Doesn't clean anything, just return copy of object
    /// </summary>
    internal sealed class DefaultCleaner: SendModelCleaner
    {
        public override Type TargetType { get; }
        public override T Clean<T>(T model, bool cleanFromSyncedFilesData)
        {
            return Clone(model);
        }
    }
}