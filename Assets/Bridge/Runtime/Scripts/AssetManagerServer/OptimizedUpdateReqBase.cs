using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    public abstract class OptimizedUpdateReqBase<T> where T:IEntity
    {
        internal abstract T OriginModel { get; }
        internal abstract T TargetModel { get; }
        internal object QueryObject { get; private set; }
        
        public abstract bool HasDataToUpdate { get; }

        internal void BuildQueryObject(bool includeFilesData)
        {
            QueryObject = BuildQueryObjectInternal(includeFilesData);
        }

        protected abstract object BuildQueryObjectInternal(bool includeFilesData);
    }
}