using Bridge.Models.Common;

namespace Bridge.Results
{ 
    public class SingleEntityResult<T>: SingleObjectResult<T> where T: IEntity
    {
        internal SingleEntityResult(T resultObject) : base(resultObject)
        {
        }

        internal SingleEntityResult(string errorMessage) : base(errorMessage)
        {
        }

        protected SingleEntityResult(bool isCanceled) : base(isCanceled)
        {
            
        }
    }
}