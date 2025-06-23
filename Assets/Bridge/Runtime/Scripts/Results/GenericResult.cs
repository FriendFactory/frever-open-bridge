namespace Bridge.Results
{
    internal sealed class GenericResult<T>: Result
    {
        public readonly T ResultObject;
        
        internal GenericResult(T resultObj)
        {
            ResultObject = resultObj;
        }
        
        internal GenericResult(string error):base(error)
        {
            
        }
    }
}