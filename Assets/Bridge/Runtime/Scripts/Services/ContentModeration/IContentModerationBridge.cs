using System.Threading.Tasks;

namespace Bridge.Services.ContentModeration
{
    public interface IContentModerationBridge
    {
        Task<ModeratedContentResult> ModerateTextContent(string text);
        Task<ModeratedContentResult> ModerateMediaContent(string uploadId, string fileExtension);
    }
}
