using System;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer.ResponseReaders;
using Bridge.Authorization;
using Bridge.Models.Common;
using Bridge.Results;

namespace Bridge.AssetManagerServer.EntityReadServices
{
    internal sealed class DefaultEntityReadService<T> : BaseEntityService, IEntityReadService<T> where T : IEntity
    {
        public DefaultEntityReadService(string host, IRequestHelper requestHelper,
            ResponseReaderProvider responseReaderProvider) : base(host, requestHelper, responseReaderProvider)
        {
        }

        public async Task<SingleEntityResult<T>> GetAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetAsyncInternal(id, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledSingleEntityResult<T>();
            }
        }

        public async Task<EntitiesResult<T>> GetAsync(Query<T> query, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.Run(() => GetWithQueryAsync(query, cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new CanceledEntitiesesResult<T>();
            }
        }

        private async Task<EntitiesResult<T>> GetEntitiesArrayAsync(string url, CancellationToken cancellationToken)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            return GetArrayResultAsync<T>(resp);
        }

        private async Task<SingleEntityResult<T>> GetAsyncInternal(long id, CancellationToken cancellationToken)
        {
            var url = GetBaseUrl<T>() + id;

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var res = await req.GetHTTPResponseAsync(cancellationToken);
            if (res.IsSuccess)
            {
                var resModel = ReadResult<T>(res);
                return new SingleEntityResult<T>(resModel);
            }

            var error = res.DataAsText;
            return new SingleEntityResult<T>(error);
        }

        private async Task<EntitiesResult<T>> GetWithQueryAsync(Query<T> q, CancellationToken cancellationToken)
        {
            var endPoint = GetBaseUrl<T>();

            endPoint = endPoint.TrimEnd('/');
            var url = endPoint + q.BuildQuery();

            return await GetEntitiesArrayAsync(url, cancellationToken);
        }
    }
}