using Bridge.Authorization;
using Bridge.Modules.Serialization;

namespace Bridge.VideoServer
{
    internal abstract class VideoService
    {
        protected readonly string Host;
        protected readonly IRequestHelper RequestHelper;
        protected readonly ISerializer Serializer;

        protected VideoService(string host, IRequestHelper requestHelper, ISerializer serializer)
        {
            Host = host;
            RequestHelper = requestHelper;
            Serializer = serializer;
        }
    }
}