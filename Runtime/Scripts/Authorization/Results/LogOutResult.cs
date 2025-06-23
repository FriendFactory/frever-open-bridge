using Bridge.Results;

namespace Bridge.Authorization.Results
{
    public sealed class LogOutResult:Result
    {
        internal LogOutResult() 
        {
        }

        internal LogOutResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}