using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Results;
using Bridge.Services.CreatePage;

namespace Bridge
{
    public interface ICreatePageBridge
    {
        Task<Result<CreatePageContentResponse>> GetCreatePageContent(string testGroup = "control", CancellationToken token = default);

        Task<EntitiesResult<Video>> GetCreatePageRowVideo(long rowId, long? targetVideoId, int takeNext,
            CancellationToken token = default);
        Task<ArrayResult<HashtagInfo>> GetCreatePageRowHashtags(long rowId, long? targetHashtagId, int takeNext, CancellationToken token = default);
        Task<ArrayResult<TemplateInfo>> GetCreatePageRowTemplates(long rowId, long? targetTemplateId, int takeNext, CancellationToken token = default);
        Task<ArrayResult<ExternalSongShortInfo>> GetCreatePageRowExternalSongs(long rowId, long? targetSongId,
            int takeNext, CancellationToken token = default);
    }
}