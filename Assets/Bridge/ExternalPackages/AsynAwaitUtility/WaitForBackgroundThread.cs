using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Bridge.ExternalPackages.AsynAwaitUtility
{
    public class WaitForBackgroundThread
    {
        public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter()
        {
            return Task.Run(() => {}).ConfigureAwait(false).GetAwaiter();
        }
    }
}
