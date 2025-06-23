using System.Threading;

namespace Bridge.Services.AssetService
{
    internal static class ThreadingExtensions
    {
        public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context)
        {
            return new SynchronizationContextAwaiter(context);
        }
    }
}