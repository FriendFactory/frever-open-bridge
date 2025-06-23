using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService;
using Bridge.Services.ContentModeration;
using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.TranscodingService
{
    internal sealed class TranscodingService : ITranscodingService
    {
        private readonly string _localSaveFolderPath = Path.Combine(Application.persistentDataPath, "Temp");
        private readonly int PING_INTERVAL_MS = 2000;
        private readonly int TIMEOUT_MS = 60000;
        private readonly string _host;
        
        private readonly IAssetService _assetService;
        private readonly ConvertedFilesCache _filesCache;
        private readonly IRequestHelper _requestHelper;
        private readonly IContentModerationService _contentModerationService;
        private readonly ISerializer _serializer;

        public TranscodingService(string host, IAssetService assetService, IRequestHelper requestHelper,
            IContentModerationService contentModerationService, ISerializer serializer)
        {
            _requestHelper = requestHelper;
            _contentModerationService = contentModerationService;
            _assetService = assetService;
            _host = Extensions.CombineUrls(host, "transcoding");
            _serializer = serializer;
            _filesCache = new ConvertedFilesCache();
        }

        public async Task<ExtractingAudioResult> ExtractAudioAsync(byte[] bytes, int durationSec)
        {
            var initTranscoding = await InitTranscoding();
            if (initTranscoding.IsError) return new ExtractingAudioResult(initTranscoding.ErrorMessage);

            var deployUrl = initTranscoding.ResultObject.TranscodingFileUploadUrl;
            var uploadFile = await UploadFileForTranscoding(deployUrl, bytes);
            if (uploadFile.IsError) return new ExtractingAudioResult(uploadFile.ErrorMessage);

            var transcodingId = initTranscoding.ResultObject.TranscodingId;
            var executeTranscoding = await ExecuteTranscoding(transcodingId, durationSec);
            if (executeTranscoding.IsError) return new ExtractingAudioResult(executeTranscoding.ErrorMessage);

            if (!executeTranscoding.ResultObject.Ok)
            {
                var errorMessage =
                    $"{executeTranscoding.ResultObject.ErrorCode}. {executeTranscoding.ResultObject.ErrorDescription}";
                return new ExtractingAudioResult(errorMessage);
            }
            
            var resultFileUrl = executeTranscoding.ResultObject.ConvertedFileUrl;
            var downloadFileInfo = await WaitAndDownloadTranscodedFile(resultFileUrl);
            if (downloadFileInfo.IsError) return new ExtractingAudioResult(downloadFileInfo.ErrorMessage);

            var filePath = downloadFileInfo.ResultObject.FilePath;
            var audioClip = await LoadUnityAudioClip(filePath);
            return new ExtractingAudioResult(audioClip, filePath, executeTranscoding.ResultObject.MediaIdentificationResultRaw);
        }

        public Task CleanCacheFromConvertedFiles()
        {
            var tasks = new[] { Task.Run(ClearLocalFiles), Task.Run(_filesCache.CleanCacheFromConvertedFilesInternal) };
            return Task.WhenAll(tasks);
        }

        private ConvertingResult TryGetFileFromCache(string filePath)
        {
            if (!File.Exists(filePath))
                return ConvertingResult.GetFailedResult($"File does not exists. File path: {filePath}");

            if (_filesCache.HasConvertedFileFor(filePath, out var cachedEntry))
                return ConvertingResult.GetSuccessResult(cachedEntry.FilePath, cachedEntry.UploadId);

            return null;
        }
        
        public async Task<ConvertingResult> ConvertAsync(Stream fileStream, string filePath, string fileExtension)
        {
            var cached = TryGetFileFromCache(filePath);
            if (cached != null && cached.IsSuccess) return cached;
            
            var initConvertingResp = await _assetService.InitConverting(fileExtension);
            if (initConvertingResp.IsError)
            {
                return ConvertingResult.GetFailedResult(initConvertingResp.ErrorMessage);
            }
            
            var uploadResult = await UploadFileForConverting(fileStream, initConvertingResp.SourceFileDestinationUrl);
            if (uploadResult.IsError)
            {
                return ConvertingResult.GetFailedResult(uploadResult.ErrorMessage);
            }

            var moderationResponse =
                await _contentModerationService.ModerateMediaContent(initConvertingResp.UploadId, fileExtension);

            if (!moderationResponse.IsSuccess)
            {
                return ConvertingResult.GetFailedResult(moderationResponse.ErrorMessage);
            }

            if (!moderationResponse.PassedModeration)
            {
                return ConvertingResult.GetFailedResult("Media can not contain inappropriate content");
            }
            
            using (var client = _requestHelper.CreateClient(false)) //todo: change to http best
            {
                return await DownloadConvertedFile(client, initConvertingResp, filePath);
            }
        }

        private async Task<ConvertingResult> DownloadConvertedFile(HttpClient client,
            ConvertingUrlResponse convertingData, string sourceFilePath)
        {
            var attempts = TIMEOUT_MS / PING_INTERVAL_MS;

            while (attempts > 0)
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Head,
                    RequestUri = new Uri(convertingData.CheckingFileExistenceUrl)
                };
                var checkingResp = await client.SendAsync(httpRequestMessage);
                if (!checkingResp.IsSuccessStatusCode)
                {
                    await Task.Delay(PING_INTERVAL_MS);

                    attempts--;
                    if (attempts == 0)
                        return ConvertingResult.GetFailedResult(
                            $"Time out error. Converting took more than {TIMEOUT_MS} ms");
                    continue;
                }

                var resp = await client.GetAsync(convertingData.ConvertedFileUrl);

                if (!resp.IsSuccessStatusCode)
                    return ConvertingResult.GetFailedResult(resp.ReasonPhrase);

                var filePath = await _filesCache.SaveFileAsync(resp.Content, convertingData.ConvertedFileExtension,
                    sourceFilePath, convertingData.UploadId);
                return ConvertingResult.GetSuccessResult(filePath, convertingData.UploadId);
            }

            throw new Exception("Unexpected error. Bridge:ConvertingServer");
        }

        private async Task<Result> UploadFileForConverting(Stream fileStream, string url)
        {
            var request = _requestHelper.CreateRequest(url, HTTPMethods.Put, false, false);

            fileStream.Seek(0, SeekOrigin.Begin);
            
            request.UploadStream = fileStream;
            request.UseUploadStreamLength = true;
            var resp = await request.GetHTTPResponseAsync();
            fileStream.Close();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult(resp.DataAsText);
        }

        private async Task<GenericResult<TranscodingInfo>> InitTranscoding()
        {
            var url = Extensions.CombineUrls(_host, "upload");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new GenericResult<TranscodingInfo>(resp.DataAsText);
            }

            var responseData = _serializer.DeserializeJson<TranscodingInfo>(resp.DataAsText);
            return new GenericResult<TranscodingInfo>(responseData);
        }

        private async Task<GenericResult<DownloadTranscodedFileInfo>> WaitAndDownloadTranscodedFile(string url)
        {
            using (var client = _requestHelper.CreateClient(false, false))
            {
                var attempts = TIMEOUT_MS / PING_INTERVAL_MS;

                while (attempts > 0)
                {
                    var getFileResp = await client.GetAsync(url);
                    attempts--;
                    if (!getFileResp.IsSuccessStatusCode)
                    {
                        await Task.Delay(PING_INTERVAL_MS);
                        if (attempts == 0)
                        {
                            return new GenericResult<DownloadTranscodedFileInfo>(
                                "Failed file transcoding. Reason: Timeout");
                        }
                    }
                    else
                    {
                        var path = await SaveFileLocally(getFileResp);
                        var result = new DownloadTranscodedFileInfo
                        {
                            FilePath = path
                        };
                        return new GenericResult<DownloadTranscodedFileInfo>(result);
                    }
                }

                throw new Exception("Unexpected error. Bridge:ConvertingServer");
            }
        }

        private async Task<string> SaveFileLocally(HttpResponseMessage resp)
        {
            if (!Directory.Exists(_localSaveFolderPath))
                Directory.CreateDirectory(_localSaveFolderPath);

            var bytes = await resp.Content.ReadAsByteArrayAsync();
            var fileName = $@"{Guid.NewGuid()}.mp3";
            var tempPath = Path.Combine(_localSaveFolderPath, fileName);

            File.WriteAllBytes(tempPath, bytes);
            return tempPath;
        }
        
        private static async Task<AudioClip> LoadUnityAudioClip(string filePath)
        {
            var unityWebRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
            await unityWebRequest.SendWebRequest();
            var audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
            return audioClip;
        }

        private async Task<GenericResult<TranscodeResult>> ExecuteTranscoding(string id, int durationSec)
        {
            var url = Extensions.CombineUrls(_host, "transcode");
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            var json = _serializer.SerializeToJson(new
            {
                TranscodingId = id,
                DurationSec = durationSec
            });
            req.AddJsonContent(json);
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess) return new GenericResult<TranscodeResult>(resp.DataAsText);

            var body = _serializer.DeserializeJson<TranscodeResult>(resp.DataAsText);
            return new GenericResult<TranscodeResult>(body);
        }

        private async Task<Result> UploadFileForTranscoding(string url, byte[] bytes)
        {
            var req = _requestHelper.CreateRequest(url, HTTPMethods.Put, false, false);
            req.RawData = bytes;
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess) return new ErrorResult(resp.DataAsText);
            return new SuccessResult();
        }

        private void ClearLocalFiles()
        {
            var dir = new DirectoryInfo(_localSaveFolderPath);
            if (!dir.Exists)
                return;
            
            dir.Delete(true);
        }

        private struct TranscodingInfo
        {
            public string TranscodingId { get; set; }

            public string TranscodingFileUploadUrl { get; set; }
        }

        private struct TranscodeResult
        {
            public bool Ok { get; set; }

            public string ErrorCode { get; set; }

            public string ErrorDescription { get; set; }

            public string ConvertedFileUrl { get; set; }
            
            public string MediaIdentificationResultRaw { get; set; }
        }

        private struct DownloadTranscodedFileInfo
        {
            public string FilePath;
        }
    }
}
