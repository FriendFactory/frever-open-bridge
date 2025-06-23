using System;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge
{
    public partial class ServerBridge
    {
        public Task<Result> AddTrackingId(string id)
        {
            return _adTrackingService.AddTrackingId(id);
        }

        public Task<Result> RemoveAllTrackingIds()
        {
            return _adTrackingService.RemoveAllTrackingIds();
        }
    }
}