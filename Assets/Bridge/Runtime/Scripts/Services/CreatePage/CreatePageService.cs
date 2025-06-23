using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.ClientServer;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.Services.CreatePage
{
    internal interface ICreatePageService
    {
        Task<Result<CreatePageContentResponse>> GetCreatePageContent(string testGroup, CancellationToken token = default);

        Task<EntitiesResult<Video>> GetCreatePageRowVideo(long rowId, long? targetVideoId, int takeNext,
            CancellationToken token);
        Task<ArrayResult<HashtagInfo>> GetCreatePageRowHashtags(long rowId, long? targetHashtagId, int takeNext, CancellationToken token);
        Task<ArrayResult<TemplateInfo>> GetCreatePageRowTemplates(long rowId, long? targetTemplateId, int takeNext, CancellationToken token);
        Task<ArrayResult<ExternalSongShortInfo>> GetCreatePageRowExternalSongs(long rowId, long? targetSongId,
            int takeNext, CancellationToken token);
    }

    internal sealed class CreatePageService : ServiceBase, ICreatePageService
    {
        private const string RootEndPoint = "create-page";
        
        public CreatePageService(IRequestHelper requestHelper, ISerializer serializer) : base(requestHelper, serializer)
        {
        }

        public CreatePageService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }
        
        public Task<Result<CreatePageContentResponse>> GetCreatePageContent(string testGroup, CancellationToken token = default)
        {
            var url = ConcatUrl(Host, $"{RootEndPoint}/content?testGroup={testGroup}");
            return SendRequestForSingleModel<CreatePageContentResponse>(url, token);
        }

        // return EntitiesResult<Video> instead of ArrayResult<Video> in order to be compatible with video loaders
        public async Task<EntitiesResult<Video>> GetCreatePageRowVideo(long rowId, long? targetVideoId, int takeNext,
            CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{RootEndPoint}/row/{rowId}/videos?target={targetVideoId}&takeNext={takeNext}");
            var result = await SendRequestForListModels<Video>(url, token);
            
            if (result.IsError)
            {
                return new EntitiesResult<Video>(result.ErrorMessage);
            }
            
            return new EntitiesResult<Video>(result.Models);
        }

        public Task<ArrayResult<HashtagInfo>> GetCreatePageRowHashtags(long rowId, long? targetHashtagId, int takeNext, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{RootEndPoint}/row/{rowId}/hashtags?target={targetHashtagId}&takeNext={takeNext}");
            return SendRequestForListModels<HashtagInfo>(url, token);
        }

        public Task<ArrayResult<TemplateInfo>> GetCreatePageRowTemplates(long rowId, long? targetTemplateId, int takeNext, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{RootEndPoint}/row/{rowId}/templates?target={targetTemplateId}&takeNext={takeNext}");
            return SendRequestForListModels<TemplateInfo>(url, token);
        }

        public Task<ArrayResult<ExternalSongShortInfo>> GetCreatePageRowExternalSongs(long rowId, long? targetSongId,
            int takeNext, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{RootEndPoint}/row/{rowId}/songs?target={targetSongId}&takeNext={takeNext}");
            return SendRequestForListModels<ExternalSongShortInfo>(url, token);
        }
    }
}