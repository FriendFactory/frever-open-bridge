using Bridge.Models.Common;

namespace Bridge.Results
{
    public sealed class CanceledEntitiesesResult<T>: EntitiesResult<T> where T:IEntity
    {
        internal CanceledEntitiesesResult() : base(true)
        {
        }
    }
}