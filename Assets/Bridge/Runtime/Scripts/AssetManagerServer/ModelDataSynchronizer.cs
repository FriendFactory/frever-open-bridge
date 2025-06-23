using Bridge.AssetManagerServer.ModelDataSynchronization;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    /// <summary>
    /// Server returns object result which contains only Id property in json.
    /// That's kind of optimization - to send only data what was added on back end side
    /// Other fields are the same as in post model, so no reason to send it.
    /// Current script allow to set Foreign Keys in models based on optimized back end response
    /// </summary>
    internal class ModelDataSynchronizer
    {
        public void Sync<T>(T source, T dest) where T: IEntity
        {
            var synchronizer = new ModelSynchronizer<T>(source, dest);
            synchronizer.Sync();
        }
    }
}