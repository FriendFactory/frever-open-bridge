using Bridge.Models.Common;

namespace Bridge.Results
{
    public class EntitiesResult<T> : ArrayResult<T> where T : IEntity
    {
        internal EntitiesResult(T[] models) : base(models)
        {
        }

        internal EntitiesResult(string errorMessage) : base(errorMessage)
        {
        }

        protected EntitiesResult(bool isCanceled) : base(true)
        {
            
        }
    }
}