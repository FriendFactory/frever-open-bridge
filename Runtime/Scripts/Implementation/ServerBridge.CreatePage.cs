using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Results;
using Bridge.Services.CreatePage;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private ICreatePageService _createPageService;
        
        public Task<Result<CreatePageContentResponse>> GetCreatePageContent(string testGroup = "control", CancellationToken token = default)
        {
            return _createPageService.GetCreatePageContent(testGroup, token);
        }

        public Task<EntitiesResult<Video>> GetCreatePageRowVideo(long rowId, long? targetVideoId, int takeNext,
            CancellationToken token)
        {
            return _createPageService.GetCreatePageRowVideo(rowId, targetVideoId, takeNext, token);
        }

        public Task<ArrayResult<HashtagInfo>> GetCreatePageRowHashtags(long rowId, long? targetHashtagId, int takeNext, CancellationToken token)
        {
            return _createPageService.GetCreatePageRowHashtags(rowId, targetHashtagId, takeNext, token);
        }

        public Task<ArrayResult<TemplateInfo>> GetCreatePageRowTemplates(long rowId, long? targetTemplateId, int takeNext, CancellationToken token)
        {
            return _createPageService.GetCreatePageRowTemplates(rowId, targetTemplateId, takeNext, token);
        }

        public Task<ArrayResult<ExternalSongShortInfo>> GetCreatePageRowExternalSongs(long rowId, long? targetSongId,
            int takeNext, CancellationToken token)
        {
            return _createPageService.GetCreatePageRowExternalSongs(rowId, targetSongId, takeNext, token);
        }
    }
}