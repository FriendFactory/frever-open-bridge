using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer
{
    internal abstract class ServiceBase
    {
        protected virtual string Host { get; }
        protected readonly IRequestHelper RequestHelper;
        protected readonly ISerializer Serializer;

        protected ServiceBase(IRequestHelper requestHelper, ISerializer serializer)
        {
            RequestHelper = requestHelper;
            Serializer = serializer;
        }
        
        protected ServiceBase(string host, IRequestHelper requestHelper, ISerializer serializer) :this(requestHelper, serializer)
        {
            Host = host;
        }

        protected string ConcatUrl(string url1, string url2)
        {
            return Extensions.CombineUrls(url1, url2);
        }
        
        protected async Task<ArrayResult<T>> SendRequestForListModels<T>(string url, CancellationToken token, object body = null, IDictionary<string, string> headers = null)
        {
            var hasBody = body != null;
            var method = hasBody ? HTTPMethods.Post : HTTPMethods.Get;
            var req = RequestHelper.CreateRequest(url, method, true, true);
            req.AddHeaders(headers);
            if (hasBody)
            {
                var json = SerializeToJson(body);
                req.AddJsonContent(json);
            }
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return new ArrayResult<T>(resp.DataAsText, resp.StatusCode);
            }

            var models = Serializer.DeserializeProtobuf<T[]>(resp.Data);
            return new ArrayResult<T>(models);
        }

        protected async Task<Result<T>> SendRequestForSingleModel<T>(string url, CancellationToken token, bool notFoundAsEmptySuccess = false, object body = null, bool useProtobuf = true)
        {
            var method = body == null ? HTTPMethods.Get : HTTPMethods.Post;
            var req = RequestHelper.CreateRequest(url, method, true, useProtobuf);
            if (body != null)
            {
                req.AddJsonContent(SerializeToJson(body));
            }
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess && !(resp.StatusCode == 404 && notFoundAsEmptySuccess))
            {
                return Result<T>.Error(resp.DataAsText, resp.StatusCode);
            }

            if (resp.StatusCode == 404)
            {
                return Result<T>.Success(default);
            }
            
            var model = useProtobuf? Serializer.DeserializeProtobuf<T>(resp.Data) : Serializer.DeserializeJson<T>(resp.DataAsText);
            return Result<T>.Success(model);
        }
        
        protected async Task<Result<ResponseModel>> SendPostRequest<ResponseModel>(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            if (body != null)
            {
                var bodyJson = SerializeToJson(body);
                req.AddJsonContent(bodyJson);
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<ResponseModel>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = DeserializeJson<ResponseModel>(resp.DataAsText);
            if (model is IOkResponse okResponse && !okResponse.Ok)
            {
                return Result<ResponseModel>.Error(okResponse.ErrorMessage, resp.StatusCode);
            }
            return Result<ResponseModel>.Success(model);
        }

        protected async Task<Result> SendPostRequest(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            if (body != null)
            {
                var bodyJson = SerializeToJson(body);
                req.AddJsonContent(bodyJson);
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }

            return new SuccessResult();
        }

        protected async Task<CountResult> SendRequestForCountModel(string url, CancellationToken token)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return CountResult.Error(resp.DataAsText, resp.StatusCode);
            }
            
            var count = DeserializeJson<int>(resp.DataAsText);
            return CountResult.Success(count);
        }

        protected async Task<Result> SendDeleteRequest(string url)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
            {
                return new SuccessResult();
            }

            return new ErrorResult(resp.DataAsText, resp.StatusCode);
        }
        
        
        protected async Task<Result<ResponseModel>> SendPutRequest<ResponseModel>(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            if (body != null)
            {
                var bodyJson = SerializeToJson(body);
                req.AddJsonContent(bodyJson);
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return Result<ResponseModel>.Error(resp.DataAsText, resp.StatusCode);
            }

            var model = DeserializeJson<ResponseModel>(resp.DataAsText);
            if (model is IOkResponse okResponse && !okResponse.Ok)
            {
                return Result<ResponseModel>.Error(okResponse.ErrorMessage, resp.StatusCode);
            }
            return Result<ResponseModel>.Success(model);
        }
        
        protected async Task<Result> SendPutRequest(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            if (body != null)
            {
                var bodyJson = SerializeToJson(body);
                req.AddJsonContent(bodyJson);
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }

            return new SuccessResult();
        }
        
        protected async Task<Result> SendPatchRequest(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Patch, true, false);
            if (body != null)
            {
                var bodyJson = SerializeToJson(body);
                req.AddJsonContent(bodyJson);
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }

            return new SuccessResult();
        }

        protected virtual string SerializeToJson(object obj)
        {
            return Serializer.SerializeToJson(obj);
        }

        protected virtual T DeserializeJson<T>(string json)
        {
            return Serializer.DeserializeJson<T>(json);
        }
    }
}
