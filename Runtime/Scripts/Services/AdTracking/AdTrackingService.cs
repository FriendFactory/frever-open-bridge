using System;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.Services.AdTracking
{
    internal sealed class AdTrackingService : ServiceBase, IAdTrackingService
    {
        public AdTrackingService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result> AddTrackingId(string id)
        {
            try
            {
                var url = ConcatUrl(Host, $"me/advertising/tracking/{id}");
                return await SendPostRequest<Result>(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? (Result)new CanceledResult()
                    : new ErrorResult(e.Message);
            }
        }

        public async Task<Result> RemoveAllTrackingIds()
        {
            try
            {
                var url = ConcatUrl(Host, "me/advertising/tracking");
                return await SendDeleteRequest(url);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? new CanceledResult()
                    : (Result)new ErrorResult(e.Message);
            }
        }
    }
}