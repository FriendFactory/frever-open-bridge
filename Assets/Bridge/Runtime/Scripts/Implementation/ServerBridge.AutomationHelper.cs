using System.Threading.Tasks;
using BestHTTP;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
#if UNITY_EDITOR
        public void RunInEditor()
        {
            ForceHttpManagerUpdating();
        }

        private async void ForceHttpManagerUpdating()
        {
            while (true)
            {
                HTTPManager.OnUpdate();
                await Task.Delay(33);
            }
        }
#endif
    }
}
