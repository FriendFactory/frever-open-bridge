using System;
using Bridge.Models.Common;

namespace Bridge.Results
{
    public class DeleteResult<T>: Result where T:IEntity
    {
        public readonly long EntityId;
        public Type DeletedEntityType => typeof(T);

        internal DeleteResult(long id)
        {
            EntityId = id;
        }

        internal DeleteResult(string errorMessage) : base(errorMessage)
        {
        }

        protected DeleteResult(long id, bool isCanceled): base(isCanceled)
        {
            EntityId = id;
        }
    }
}