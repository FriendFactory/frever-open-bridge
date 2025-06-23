using System.Threading.Tasks;

namespace Bridge.Services.ContentModeration
{
    internal interface IContentModerationService
    {
        Task<ModeratedContentResult> ModerateTextContent(string text);
        Task<ModeratedContentResult> ModerateMediaContent(string uploadId, string fileExtension);
    }
}
