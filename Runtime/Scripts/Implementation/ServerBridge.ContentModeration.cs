using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services.ContentModeration;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ModeratedContentResult> ModerateTextContent(string text)
        {
            return _contentModerationService.ModerateTextContent(text);
        }

        public Task<ModeratedContentResult> ModerateMediaContent(string uploadId, string fileExtension)
        {
            return _contentModerationService.ModerateMediaContent(uploadId, fileExtension);
        }
    }
}
