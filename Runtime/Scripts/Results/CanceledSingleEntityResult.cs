using Bridge.Models.Common;

namespace Bridge.Results
{
    public sealed class CanceledSingleEntityResult<T> : SingleEntityResult<T> where T: IEntity
    {
        internal CanceledSingleEntityResult() : base(true)
        {
        }
    }
}