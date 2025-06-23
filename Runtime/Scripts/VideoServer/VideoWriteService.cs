using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.Forms;
using Bridge.Authorization;
using Bridge.Models;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.VideoServer.InternalModels;


namespace Bridge.VideoServer
{
    internal sealed class VideoWriteService : VideoService
    {
        private const int VIDEO_FILE_UPLOADING_TIMEOUT_SEC = 360;
        private const int VIDEO_COMPLETE_UPLOADING_TIMEOUT_SEC = 360;//should be reduced to default 60 when we deploy video queue on backend side
        
        public VideoWriteService(string host, IRequestHelper requestHelper, ISerializer serializer)
            : base(host, requestHelper, serializer)
        {
        }

        public async Task<VideoUploadResult> UploadLevelVideoAsync(DeployLevelVideoReq req)
        {
            ThrowExceptionIfFileDoesNotExist(req.LocalPath);
            
            var initUploadingResp = await InitUploading(req.LocalPath);

            if (!initUploadingResp.IsSuccess)
                return new VideoUploadResult(initUploadingResp.ErrorData);
            
            var requestBody = new CompleteLevelVideoUploadingRequest
            {
                LevelId = req.LevelId,
                Size = GetFileSize(req.LocalPath),
                DurationSec = req.DurationSec,
                IsPublic = req.IsPublic,
                Description = req.VideoDescription,
                VideoOrientation = req.VideoOrientation,
                Access = req.Access,
                CreateTemplate = req.GenerateTemplate,
                CreateTemplateWithName = req.GenerateTemplateWithName,
                TaggedFriendIds = req.TaggedFriendIds,
                Links = req.Links,
                PublishTypeId = req.PublishTypeId,
                IsVideoMessage = req.PublishTypeId == ServerConstants.VideoPublishingType.VIDEO_MESSAGE,
                AllowRemix = req.AllowRemix,
                AllowComment = req.AllowComment,
            };
            
            return await CompleteUploading(initUploadingResp.UploadId, "upload", Serializer.SerializeToJson(requestBody));
        }

        public async Task<VideoUploadResult> UploadNonLevelVideoAsync(DeployNonLevelVideoReq req)
        {
            ThrowExceptionIfFileDoesNotExist(req.LocalPath);

            var initUploadingResp = await InitUploading(req.LocalPath);

            if (!initUploadingResp.IsSuccess)
                return new VideoUploadResult(initUploadingResp.ErrorData);

            var requestBody = new CompleteNonLevelVideoUploadingRequest
            {
                DurationSec = req.DurationSec,
                Size = GetFileSize(req.LocalPath),
                IsPublic = req.IsPublic,
                Access = req.Access,
                Description = req.Description,
                IsVideoMessage = req.PublishTypeId == ServerConstants.VideoPublishingType.VIDEO_MESSAGE,
                TaggedFriendIds = req.TaggedFriendIds,
                Links = req.Links,
                PublishTypeId = req.PublishTypeId,
                AllowRemix = req.AllowRemix,
                AllowComment = req.AllowComment,
            };
            
            return await CompleteUploading(initUploadingResp.UploadId, "upload-non-level", Serializer.SerializeToJson(requestBody));
        }
        
        public async Task<VideoUploadResult> UploadNonLevelAiVideoAsync(DeployNonLevelVideoReq req)
        {
            ThrowExceptionIfFileDoesNotExist(req.LocalPath);

            var initUploadingResp = await InitUploading(req.LocalPath);

            if (!initUploadingResp.IsSuccess)
                return new VideoUploadResult(initUploadingResp.ErrorData);

            var requestBody = new CompleteNonLevelVideoUploadingRequest
            {
                DurationSec = req.DurationSec,
                Size = GetFileSize(req.LocalPath),
                IsPublic = req.IsPublic,
                Access = req.Access,
                Description = req.Description,
                IsVideoMessage = req.PublishTypeId == ServerConstants.VideoPublishingType.VIDEO_MESSAGE,
                TaggedFriendIds = req.TaggedFriendIds,
                Links = req.Links,
                PublishTypeId = req.PublishTypeId,
                AllowRemix = req.AllowRemix,
                AllowComment = req.AllowComment,
            };
            
            return await CompleteUploading(initUploadingResp.UploadId, "upload-non-level", Serializer.SerializeToJson(requestBody), true);
        }
        
        public Task<Result> GenerateLipSync(long? videoId, LipSyncRequest lipSyncRequest)
        {
            var url = Extensions.CombineUrls(Host, $"style-transformation/{videoId}/lip-sync");
            return PostAsync(url, lipSyncRequest);
        }
        
        public Task<Result> LikeVideoAsync(long videoId)
        {
            return ChangeLikeState(videoId, true);
        }

        public Task<Result> UnlikeVideoAsync(long videoId)
        {
            return ChangeLikeState(videoId, false);
        }

        public Task<Result> ChangePrivacyAsync(long videoId, UpdateVideoAccessRequest model)
        {
            return PostAsync(GetPublishingUrl(videoId), model);
        }

        public Task<Result> DeleteVideo(long videoId)
        {
            return Delete(videoId, false);
        }

        public Task<Result> DeleteVideoByLevelId(long levelId)
        {
            return Delete(levelId, true);
        }

        public Task<Result> RateVideoList(RateVideoRequest requestModel)
        {
            var url = Extensions.CombineUrls(Host, "rating");
            return PostAsync(url, requestModel);
        }

        private Task<Result> ChangeLikeState(long videoId, bool like)
        {
            return PostAsync(GetLikesUrl(videoId, like));
        }

        private string GetPublishingUrl(long videoId)
        {
            return Path.Combine(Host, $"video/{videoId}/access").FixUrlSlashes();
        }

        private string GetLikesUrl(long videoId, bool like)
        {
            var likeRoute = like ? "like" : "unlike";
            return Path.Combine(Host, $"video/{videoId}/{likeRoute}").FixUrlSlashes();
        }
        
        private string GetCommentLikesUrl(long videoId, long commentId)
        {
            return Path.Combine(Host, $"video/{videoId}/comment/{commentId}/like").FixUrlSlashes();
        }

        private async Task<Result> PostAsync(string url, object body = null)
        {
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            if (body != null)
            {
                req.AddJsonContent(Serializer.SerializeToJson(body));
            }
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new ErrorResult(resp.DataAsText);

            return new SuccessResult();
        }

        private async Task<InitVideoUploadingResponse> InitUploading(string fileLocalPath)
        {
            var url = Path.Combine(Host, "video/upload").FixUrlSlashes();
            var initReq = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var initResp = await initReq.GetHTTPResponseAsync();
            var respJson = initResp.DataAsText;
            var respData = Serializer.DeserializeJson<VideoUploadReq>(respJson);
            return await UploadVideo(respData, fileLocalPath);
        }

        private async Task<InitVideoUploadingResponse> UploadVideo(VideoUploadReq videoInitUpload, string fileLocalPath)
        {
            var videoUploadReq =
                RequestHelper.CreateRequest(videoInitUpload.UploadUrl, HTTPMethods.Put, false, false);
            videoUploadReq.UploadStream = File.OpenRead(fileLocalPath);
            videoUploadReq.DisposeUploadStream = true;
            videoUploadReq.Timeout = TimeSpan.FromSeconds(VIDEO_FILE_UPLOADING_TIMEOUT_SEC);

            var videoUploadResp = await videoUploadReq.GetHTTPResponseAsync();

            if (!videoUploadResp.IsSuccess)
            {
                var errorData = new ErrorData((HttpStatusCode) videoUploadResp.StatusCode,
                    videoUploadResp.DataAsText);
                return new InitVideoUploadingResponse(errorData);
            }

            return new InitVideoUploadingResponse(videoInitUpload.UploadId);
        }

        private async Task<VideoUploadResult> CompleteUploading(string uploadId, string endPoint, string bodyJson, bool isAiVideo = false)
        {
            var url = Path.Combine(Host, $"video/{endPoint}/", uploadId).FixUrlSlashes();
            if (isAiVideo)
            {
                url = Path.Combine(url, "ai").FixUrlSlashes();
            }

            var req = RequestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            req.AddJsonContent(bodyJson);
            req.Timeout = TimeSpan.FromSeconds(VIDEO_COMPLETE_UPLOADING_TIMEOUT_SEC);

            var uploadingFileResp = await req.GetHTTPResponseAsync();
            if (!uploadingFileResp.IsSuccess)
            {
                var errorMessage = uploadingFileResp.DataAsText;
                return new VideoUploadResult((HttpStatusCode) uploadingFileResp.StatusCode, errorMessage);
            }

            var responseString = uploadingFileResp.DataAsText;
            var video = long.Parse(responseString);

            return new VideoUploadResult(video);
        }

        public async Task<SingleEntityResult<CommentInfo>> AddComment(long videoId, AddCommentRequest requestModel)
        {
            var url = new Uri(
                new Uri(Host, UriKind.Absolute),
                new Uri($"video/{videoId}/comment", UriKind.Relative)
            );
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            req.AddJsonContent(Serializer.SerializeToJson(requestModel));
            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new SingleEntityResult<CommentInfo>(resp.DataAsText);
            }

            var model = Serializer.DeserializeProtobuf<CommentInfo>(resp.Data);
            return new SingleEntityResult<CommentInfo>(model);
        }

        public async Task<Result> LikeCommentAsync(long videoId, long commentId)
        {
            var url = GetCommentLikesUrl(videoId, commentId);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult(resp.DataAsText);
        }

        public async Task<Result> UnlikeCommentAsync(long videoId, long commentId)
        {
            var url = GetCommentLikesUrl(videoId, commentId);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult(resp.DataAsText);
        }
        
        private async Task<Result> Delete(long id, bool byLevelId)
        {
            var url = GetDeleteUrl(id, byLevelId);
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Delete, true, false);
            var resp = await req.GetHTTPResponseAsync();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult(resp.DataAsText);
        }

        public async Task<Result<TemplateInfo>> GenerateTemplate(long templateFromVideoId, string generateTemplateWithName)
        {
            try
            {
                var url = new Uri(
                    new Uri(Host, UriKind.Absolute),
                    new Uri($"template/my", UriKind.Relative)
                );
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
                var json = Serializer.SerializeToJson(new
                {
                    videoId = templateFromVideoId,
                    templateName = generateTemplateWithName
                });
                req.AddJsonContent(json);
                var resp = await req.GetHTTPResponseAsync();
                
                if (resp.IsSuccess)
                {
                    var resModel = Serializer.DeserializeJson<TemplateInfo>(resp.DataAsText);
                    return Result<TemplateInfo>.Success(resModel);
                }

                var error = resp.DataAsText;
                return Result<TemplateInfo>.Error(error);
            }
            catch (OperationCanceledException)
            {
                return Result<TemplateInfo>.Cancelled();
            }
        }
        
        public async Task<Result<long?>> CheckTemplateName(string templateName)
        {
            try
            {
                var url = new Uri(
                    new Uri(Host, UriKind.Absolute),
                    new Uri($"template/check-name", UriKind.Relative)
                );
                var req = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
                var json = Serializer.SerializeToJson(new
                {
                    name = templateName
                });
                req.AddJsonContent(json);
                var resp = await req.GetHTTPResponseAsync();
                
                if (resp.IsSuccess)
                {
                    var resModel = Serializer.DeserializeJson<CheckTemplateNameResponse>(resp.DataAsText);
                    return Result<long?>.Success(resModel.templateId);
                }

                var error = resp.DataAsText;
                return Result<long?>.Error(error);
            }
            catch (OperationCanceledException)
            {
                return Result<long?>.Cancelled();
            }
        }

        private string GetDeleteUrl(long id, bool byLevelId)
        {
            var url = Path.Combine(Host, "video");
            if (byLevelId)
                url += "/level";

            url += $"/{id}";
            return url.FixUrlSlashes();
        }

        public async Task<Result> SendViewsData(ICollection<VideoView> views)
        {
            var url = Path.Combine(Host, "video/views");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var json = Serializer.SerializeToJson(views);
            request.AddJsonContent(json);

            var resp = await request.GetHTTPResponseAsync();
            if (resp.IsSuccess)
                return new SuccessResult();
            return new ErrorResult(resp.DataAsText);
        }

        public async Task<Result> ClaimVideoShareReward(string videoGuid)
        {
            var url = Path.Combine(Host, $"video/share/{videoGuid}");
            var result = await PostAsync(url);
            if (result.IsError)
            {
                return new ErrorResult(result.ErrorMessage);
            }

            return new SuccessResult();
        }

        private class InitVideoUploadingResponse
        {
            public readonly ErrorData ErrorData;
            public readonly string UploadId;

            public InitVideoUploadingResponse(string uploadId)
            {
                UploadId = uploadId;
            }

            public InitVideoUploadingResponse(ErrorData errorData)
            {
                ErrorData = errorData;
            }

            public bool IsSuccess => !string.IsNullOrEmpty(UploadId);
        }

        private void ThrowExceptionIfFileDoesNotExist(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Video does not exist at path {filePath}");
        }

        private long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }
        
        internal class CheckTemplateNameResponse
        {
            public bool isUnique;
            public long? templateId;
        }
    }
    
    public class UpdateVideoAccessRequest
    {
        public VideoAccess Access { get; set; }

        public long[] TaggedFriendIds { get; set; }
    }
    
    public class RateVideoRequest 
    {
        public long RaterLevelId { get; set; }
        public long RatedVideoId { get; set; }
        public int Rating { get; set; }
    }
}