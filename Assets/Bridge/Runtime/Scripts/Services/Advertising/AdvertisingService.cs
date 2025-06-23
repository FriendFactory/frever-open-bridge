using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.Services.Advertising
{
    internal sealed class AdvertisingService : AssetServiceBase, IAdvertisingService
    {
        public AdvertisingService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host,
            requestHelper, serializer) { }

        public Task<ArrayResult<PromotedSong>> GetPromotedSongs(int take, int skip, CancellationToken token)
        {
            var url = BuildUrl($"promoted-song?take={take}&skip={skip}");
            
            return SendRequestForListModels<PromotedSong>(url, token);
        }
    }
}