using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Template;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Template
{
    internal sealed class TemplateService: ServiceBase, ITemplateService
    {
        public TemplateService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<TemplateInfo>> GetDiscoveryEventTemplates(int top, int skip, int? characterCount, CancellationToken cancellationToken)
        {
            try
            {
                return await GetDiscoveryInternal(top, skip, characterCount, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }
        
        public async Task<ArrayResult<TemplateInfo>> GetTrendingEventTemplates(int top, int skip, int? characterCount, CancellationToken cancellationToken)
        {
            try
            {
                return await GetTrendingInternal(top, skip, characterCount, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<TemplateInfo>> GetPersonalEventTemplates(int top, int skip, IDictionary<string,string> headers, CancellationToken token = default)
        {
            try
            {
                return await GetPersonalInternal(top, skip, headers, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<TemplateInfo>> GetEventTemplates(int top, int skip, long? categoryId,
            long? subCategoryId, int? characterCount, string filter,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetEventTemplatesInternal(top, skip, categoryId, subCategoryId, characterCount, filter, cancellationToken);

            }
            catch (Exception)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }

        public async Task<Result<TemplateInfo>> GetEventTemplate(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await GetEventTemplateInternal(id, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return Result<TemplateInfo>.Cancelled();
            }   
        }

        public async Task<Result<LevelFullData>> GetEventForEventTemplate(long templateId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetEventForEventTemplateInternal(templateId, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplates(int top, int skip, CancellationToken cancellationToken)
        {
            try
            {
                return await GetOnBoardingInternal(top, skip, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplatesRandomized(CancellationToken cancellationToken)
        {
            try
            {
                return await GetOnBoardingRandomizedInternal(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<TemplateInfo>.Cancelled();
            }
        }

        public async Task<Result<TemplateInfo>> RenameTemplate(long templateId, string newName)
        {
            try
            {
                var req = RequestHelper.CreateRequest(BuildUrl($"{templateId}"), HTTPMethods.Put, true, true);
                var json = Serializer.SerializeToJson(new {name = newName});
                req.AddJsonContent(json);
                var resp = await req.GetHTTPResponseAsync();
                
                if (resp.IsSuccess)
                {
                    var resModel = Serializer.DeserializeProtobuf<TemplateInfo>(resp.Data);
                    return Result<TemplateInfo>.Success(resModel);
                }

                var error = resp.DataAsText;
                return Result<TemplateInfo>.Error(error);
            }
            catch (OperationCanceledException)
            {
                return Result<TemplateInfo>.Cancelled();
            }
        }

        private Task<ArrayResult<TemplateInfo>> GetDiscoveryInternal(int top, int skip, int? characterCount, CancellationToken cancellationToken)
        {
            var url = BuildUrl("discovery", top, skip);
            if (characterCount.HasValue)
                url = AppendCharacterCountFilter(url, characterCount.Value);
            return GetEntitiesArrayAsync(url, cancellationToken);
        }

        private async Task<ArrayResult<TemplateInfo>> GetEntitiesArrayAsync(string url, CancellationToken cancellationToken, IDictionary<string,string> headers = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            req.AddHeaders(headers);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            if (!resp.IsSuccess)
            {
                return ArrayResult<TemplateInfo>.Error(resp.DataAsText);
            }

            var models = Serializer.DeserializeProtobuf<TemplateInfo[]>(resp.Data);
            return new ArrayResult<TemplateInfo>(models);
        }

        private Task<ArrayResult<TemplateInfo>> GetOnBoardingRandomizedInternal(CancellationToken cancellationToken)
        {
            var url = BuildUrl("on-boarding/randomized");
            return GetEntitiesArrayAsync(url, cancellationToken);
        }

        private Task<ArrayResult<TemplateInfo>> GetOnBoardingInternal(int top, int skip, CancellationToken cancellationToken)
        {
            var url = BuildUrl("on-boarding", top, skip);
            return GetEntitiesArrayAsync(url, cancellationToken);
        }
        
        private Task<ArrayResult<TemplateInfo>> GetEventTemplatesInternal(int top, int skip, long? categoryId, long? subCategoryId, int? characterCount,
            string filter, CancellationToken cancellationToken)
        {
            var topSkipUrl = BuildUrl(string.Empty, top, skip);

            if (categoryId.HasValue) topSkipUrl += $"&categoryId={categoryId.Value}";
            if (subCategoryId.HasValue) topSkipUrl += $"&subCategoryId={subCategoryId.Value}";
            if (characterCount.HasValue) topSkipUrl = AppendCharacterCountFilter(topSkipUrl, characterCount.Value);
            if (!string.IsNullOrEmpty(filter)) topSkipUrl += $"&filter={filter}";
            
            return GetEntitiesArrayAsync(topSkipUrl, cancellationToken);
        }
        
        private Task<ArrayResult<TemplateInfo>> GetTrendingInternal(int top, int skip, int? characterCount, CancellationToken cancellationToken)
        {
            var url = BuildUrl("trending", top, skip);
            if (characterCount.HasValue)
                url = AppendCharacterCountFilter(url, characterCount.Value);
            return GetEntitiesArrayAsync(url, cancellationToken);
        }
        
        private Task<ArrayResult<TemplateInfo>> GetPersonalInternal(int top, int skip,
            IDictionary<string, string> headers, CancellationToken cancellationToken)
        {
            var url = BuildUrl("personal", top, skip);
            return GetEntitiesArrayAsync(url, cancellationToken, headers);
        }
        
        private string BuildUrl(string endPoint, int top, int skip)
        {
            return ConcatUrl(BuildUrl(endPoint), $"?top={top}&skip={skip}");
        }

        private string BuildUrl(string endPoint)
        {
           return ConcatUrl(Host, $"{nameof(Models.AsseManager.Template)}/{endPoint}");
        }

        private string AppendCharacterCountFilter(string baseUrl, int characterCount)
        {
            return $"{baseUrl}&characterCount={characterCount}";
        }
        
        private async Task<Result<TemplateInfo>> GetEventTemplateInternal(long id, CancellationToken cancellationToken)
        {
            var url = ConcatUrl(Host, $"{nameof(Models.AsseManager.Template)}/{id}");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(cancellationToken);
            if (resp.IsSuccess)
            {
                var resModel = Serializer.DeserializeProtobuf<TemplateInfo>(resp.Data);
                return Result<TemplateInfo>.Success(resModel);
            }

            var error = resp.DataAsText;
            return Result<TemplateInfo>.Error(error);
        }

        private async Task<Result<LevelFullData>> GetEventForEventTemplateInternal(long templateId,
            CancellationToken cancellationToken = default)
        {
            var url = ConcatUrl(Host, $"{nameof(Models.AsseManager.Template)}/{templateId}/level");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await request.GetHTTPResponseAsync(token: cancellationToken);
            if (!resp.IsSuccess)
            {
                return Result<LevelFullData>.Error(resp.DataAsText);
            }

            var model = Serializer.DeserializeProtobuf<LevelFullData>(resp.Data);
            return Result<LevelFullData>.Success(model);
        }
    }
}