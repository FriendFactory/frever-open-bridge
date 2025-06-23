using System;
using Bridge.Authorization;
using Bridge.Models.Common.Files;
using Bridge.Services.AssetService.DownloadRequests;

namespace Bridge.Services.AssetService
{
    internal sealed class AssetDownloadRequestProvider
    {
        private readonly IRequestHelper _requestHelper;

        public AssetDownloadRequestProvider(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public DownloadRequest GetDownloadRequest(FileExtension fileExtension)
        {
            switch (fileExtension)
            {
                case FileExtension.Null:
                    throw new InvalidOperationException("File extension must not be null");
                case FileExtension.Mp3:
                    return new Mp3DownloadRequest();
                case FileExtension.Ogg:
                    return new OggDownloadRequest();
                case FileExtension.Wav:
                    return new WavDownloadRequest();
                case FileExtension.Gif:
                    return new GifDownloadRequest(_requestHelper);
                case FileExtension.Png:
                    return new ImageDownloadRequest(_requestHelper);
                case FileExtension.Txt:
                    return new TextFileDownloadRequest(_requestHelper);
                case FileExtension.Empty:
                    return new AssetBundleDownloadRequest(_requestHelper);
                case FileExtension.Jpg:
                    return new ImageDownloadRequest(_requestHelper);
                case FileExtension.Mp4:
                    return new VideoDownloadRequest();
                case FileExtension.Jpeg:
                    return new ImageDownloadRequest(_requestHelper);
                case FileExtension.Mov:
                    return new VideoDownloadRequest();
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileExtension), fileExtension, null);
            }
        }
    }
}