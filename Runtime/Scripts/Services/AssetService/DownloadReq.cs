using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Services.AssetService
{
    internal struct DownloadReq
    {
        public readonly IFilesAttachedEntity Model;
        public readonly FileInfo FileInfo;
        public readonly bool PrefetchOnly;
        public readonly StreamingType? StreamingType;
        public readonly bool CacheFile;

        public DownloadReq(IFilesAttachedEntity model, FileInfo fileInfo, bool prefetchOnly, StreamingType? streamingType, bool cacheFile)
        {
            Model = model;
            FileInfo = fileInfo;
            PrefetchOnly = prefetchOnly;
            StreamingType = streamingType;
            CacheFile = cacheFile;
        }
    }
}