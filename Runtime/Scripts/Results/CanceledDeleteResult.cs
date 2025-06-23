using Bridge.Models.Common;

namespace Bridge.Results
{
    public sealed class CanceledDeleteResult<T>: DeleteResult<T> where T:IEntity
    {
        public CanceledDeleteResult(long id) : base(id, true)
        {
        }
    }
}