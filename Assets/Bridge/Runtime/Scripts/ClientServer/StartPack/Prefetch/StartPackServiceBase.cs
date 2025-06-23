using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.StartPack.Prefetch;
using Bridge.Modules.Serialization;

namespace Bridge.ClientServer.StartPack.Prefetch
{
    internal abstract class StartPackServiceBase<T>: ServiceBase where T:IStartPack
    {
        private const string ROOT_END_POINT = "start-pack";

        protected string EndPoint =>  $"/{ROOT_END_POINT}/{EndPointName}";
        protected abstract string EndPointName { get; }

        protected StartPackServiceBase(string host, IRequestHelper requestHelper, ISerializer serializer): base(host, requestHelper, serializer)
        {
        }

        public async Task<StartPackResult<T>> GetStartPackAsync(IDictionary<string, string> headers = null, CancellationToken token = default)
        {
            var url = Extensions.CombineUrls(Host, EndPoint);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            req.AddHeaders(headers);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess) return StartPackResult<T>.Failed(resp.DataAsText);

            var pack = ReadResponse(resp);
            return StartPackResult<T>.Success(pack);
        }

        protected abstract T ReadResponse(HTTPResponse response);
    }
}