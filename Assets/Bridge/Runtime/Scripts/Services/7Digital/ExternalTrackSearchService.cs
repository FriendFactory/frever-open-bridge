using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services._7Digital.Models;

namespace Bridge.Services._7Digital
{
    internal class ExternalTrackSearchService : ServiceBase, IExternalTrackSearchService 
    {
        public ExternalTrackSearchService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer) { }
        
        public async Task<ArrayResult<TrackInfo>> SearchExternalTracks(string searchQuery, int takeNext = 10, int skip = 0,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var parameters = $"q={searchQuery}&take={takeNext.ToString()}&skip={skip.ToString()}";
                var url = ConcatUrl(Host, $"music/search?{parameters}");
                
                var result = await SendRequestForListModels<TrackInfo>(url, cancellationToken);

                return result;
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TrackInfo>.Cancelled();
            }
            catch (Exception e)
            {
                return ArrayResult<TrackInfo>.Error(e.Message);
            }
        }
    }
}