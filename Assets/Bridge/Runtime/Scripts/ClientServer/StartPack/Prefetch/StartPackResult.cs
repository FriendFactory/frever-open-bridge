using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Results;
using Bridge;

namespace Bridge.ClientServer.StartPack.Prefetch
{
    public sealed class StartPackResult<T>: Result where T: IStartPack
    {
        public readonly T Pack;

        private StartPackResult(T pack)
        {
            Pack = pack;
        }

        private StartPackResult(bool isCanceled) : base(isCanceled)
        {
        }

        private StartPackResult(string errorMessage) : base(errorMessage)
        {
        }
        
        internal static StartPackResult<T> Success(T pack)
        {
            return new StartPackResult<T>(pack);
        }

        internal static StartPackResult<T> Canceled()
        {
            return new StartPackResult<T>(true);
        }

        internal static StartPackResult<T> Failed(string reason)
        {
            return new StartPackResult<T>(reason);
        }
    }
}