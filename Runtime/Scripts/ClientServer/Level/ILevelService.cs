using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.AssetManagerServer;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Level;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Level.Shuffle;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService.Caching;

namespace Bridge.ClientServer.Level
{
    internal interface ILevelService
    {
        Task<ArrayResult<LevelShortInfo>> GetLevelDrafts(int take, int skip, CancellationToken token = default);
        Task<Result<LevelShortInfo>> GetLevelThumbnailInfo(long levelId, CancellationToken token = default);
        Task<Result<LevelFullData>> GetLevel(long levelId, CancellationToken token = default);
        Task<Result<LevelFullData>> GetLevelTemplateForVideoMessage(CancellationToken token = default);
        Task<Result<LevelFullData>> GetShuffledLevel(long levelId, CancellationToken token = default);
        Task<Result<ShuffleMLResult>> GetShuffledLevelRemixAI(MlRemixRequest model, CancellationToken token = default);
        Task<Result<LevelShuffleResult>> GetShuffledLevel(LevelShuffleInput levelData, CancellationToken token = default);
        Task<Result<LevelShuffleResult>> GetShuffledLevelAI(LevelShuffleInputAI levelData, CancellationToken token = default);
        Task<Result<LevelFullData>> SaveLevel(LevelFullInfo level);
        Task<Result> UpdateLevelDescription(long levelId, string description);
        Task<UpdateFilesResult> UpdateEventThumbnails(Dictionary<long, List<FileInfo>> eventThumbnailData);
        Task<UpdateFilesResult> UpdateEventCameraAnimations(Dictionary<long, List<FileInfo>> eventThumbnailData);
        Task<Result> DeleteLevel(long levelId);
    }

    internal sealed class LevelService : FilesUploadingServiceBase<LevelFullData, LevelFullInfo>, ILevelService
    {
        private const string LevelEndPoint = "Level";

        public LevelService(string host, IRequestHelper requestHelper, ISerializer serializer,
            ModelsFileUploader filesUploader, AssetsCache assetsCache)
            : base(host, requestHelper, serializer, filesUploader, assetsCache)
        {
        }

        public async Task<ArrayResult<LevelShortInfo>> GetLevelDrafts(int take, int skip,
            CancellationToken token = default)
        {
            try
            {
                return await GetLevelDraftsInternal(take, skip, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<LevelShortInfo>();
            }
        }

        public async Task<Result<LevelShortInfo>> GetLevelThumbnailInfo(long levelId, CancellationToken token = default)
        {
            try
            {
                return await GetLevelThumbnailInfoInternal(levelId, token);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelShortInfo>.Cancelled();
            }
        }

        public async Task<Result<LevelFullData>> GetLevel(long levelId, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/{levelId}");
                return await SendRequestForSingleModel<LevelFullData>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelFullData>.Cancelled();
            }
        }

        public async Task<Result<LevelFullData>> GetLevelTemplateForVideoMessage(CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/video-message");
                return await SendRequestForSingleModel<LevelFullData>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelFullData>.Cancelled();
            }
        }

        public async Task<Result<LevelFullData>> GetShuffledLevel(long levelId, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/remix/{levelId}/shuffle");
                return await SendRequestForSingleModel<LevelFullData>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelFullData>.Cancelled();
            }
        }

        public async Task<Result<ShuffleMLResult>> GetShuffledLevelRemixAI(MlRemixRequest model, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/shuffle/ai");
                var resp = await SendPostRequest<ShuffledMlResponse>(url, model);
                if (resp.IsError)
                {
                    return Result<ShuffleMLResult>.Error(resp.ErrorMessage);
                }

                const float timeOutSec = 90f;
                const float attemptIntervalSec = 3f;
                var attemptCount = timeOutSec / attemptIntervalSec;
                while (attemptCount-- >= 0)
                {
                    if (token.IsCancellationRequested) return Result<ShuffleMLResult>.Cancelled();
                    url = ConcatUrl(Host, $"{LevelEndPoint}/shuffle/ai/poll-status/{resp.Model.RemixRequestId}");
                    var shuffledLevelStatus = await SendPostRequest<ShuffleMLResult>(url);
                    if (shuffledLevelStatus.IsError) continue;
                    if (!shuffledLevelStatus.Model.IsReady) continue;
                    return Result<ShuffleMLResult>.Success(shuffledLevelStatus.Model);
                }

                return Result<ShuffleMLResult>.Error("Time out");

            }
            catch (OperationCanceledException)
            {
                return Result<ShuffleMLResult>.Cancelled();
            }
            
        }
        
        private sealed class ShuffledMlResponse
        {
            public string RemixRequestId { get; set; }
        }

        public async Task<Result<LevelShuffleResult>> GetShuffledLevel(LevelShuffleInput levelData, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/shuffle");
                return await SendRequestForSingleModel<LevelShuffleResult>(url, token, body: levelData);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelShuffleResult>.Cancelled();
            }
        }

        public async Task<Result<LevelShuffleResult>> GetShuffledLevelAI(LevelShuffleInputAI levelData, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"{LevelEndPoint}/shuffle/level/ai");
                return await SendRequestForSingleModel<LevelShuffleResult>(url, token, body: levelData);
            }
            catch (OperationCanceledException)
            {
                return Result<LevelShuffleResult>.Cancelled();
            }
        }

        public Task<Result<LevelFullData>> SaveLevel(LevelFullInfo level)
        {
            return SendModel(level, LevelEndPoint);
        }

        public async Task<Result> UpdateLevelDescription(long levelId, string description)
        {
            var url = ConcatUrl(Host, $"{LevelEndPoint}/description");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var json = Serializer.SerializeToJson(new
            {
                Id = levelId,
                Description = description
            });
            request.AddJsonContent(json);
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new ErrorResult(resp.DataAsText, resp.StatusCode);
            }
            return new SuccessResult();
        }

        public Task<UpdateFilesResult> UpdateEventThumbnails(Dictionary<long, List<FileInfo>> eventThumbnailData)
        {
            return UpdateEventFiles("events/thumbnails", eventThumbnailData, typeof(EventFullInfo));
        }
        
        public Task<UpdateFilesResult> UpdateEventCameraAnimations(Dictionary<long, List<FileInfo>> eventThumbnailData)
        {
            return UpdateEventFiles("events/camera-animations", eventThumbnailData, typeof(CameraAnimationFullInfo));
        }

        public Task<Result> DeleteLevel(long levelId)
        {
            var url = ConcatUrl(Host, $"{LevelEndPoint}/{levelId}/mark-deleted");
            return SendDeleteRequest(url);
        }

        private async Task<Result<LevelShortInfo>> GetLevelThumbnailInfoInternal(long levelId, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{LevelEndPoint}/{levelId}/info");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await request.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess) return Result<LevelShortInfo>.Error(resp.DataAsText, resp.StatusCode);

            var shortInfo = Serializer.DeserializeProtobuf<LevelShortInfo>(resp.Data);
            return Result<LevelShortInfo>.Success(shortInfo);
        }

        private async Task<ArrayResult<LevelShortInfo>> GetLevelDraftsInternal(int take, int skip,
            CancellationToken token)
        {
            var url = ConcatUrl(Host, $"{LevelEndPoint}/drafts?skip={skip}&take={take}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await request.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess) return new ArrayResult<LevelShortInfo>(resp.DataAsText, resp.StatusCode);

            var drafts = Serializer.DeserializeProtobuf<LevelShortInfo[]>(resp.Data);
            return new ArrayResult<LevelShortInfo>(drafts);
        }

        protected override ICollection<FileInfo> CollectFiles(LevelFullInfo model)
        {
            var output = new List<FileInfo>();
            output.AddRange(SelectEventsThumbnails(model));
            output.AddRange(SelectCameraAnimations(model));
            output.AddRange(SelectVoiceTracks(model));
            output.AddRange(SelectFaceAnimations(model));
            output.AddRange(SelectPhotoAndVideoFiles(model));
            return output;
        }

        protected override async Task MoveUploadedFilesToCache(LevelFullInfo sendModel, LevelFullData response)
        {
            var sentEvents = sendModel.Event.OrderBy(x => x.LevelSequence).ToArray();
            var respEvents = response.Level.Event.OrderBy(x => x.LevelSequence).ToArray();
            for (var i = 0; i < sentEvents.Length; i++)
            {
                await SaveFilesToCache(sentEvents[i], respEvents[i]);
            }
        }

        private static IEnumerable<FileInfo> SelectFaceAnimations(LevelFullInfo level)
        {
            return level.Event.SelectMany(x => x.CharacterController).Select(x => x.FaceVoice)
                .Where(x => x.FaceAnimation != null).SelectMany(x => x.FaceAnimation.Files);
        }

        private static IEnumerable<FileInfo> SelectVoiceTracks(LevelFullInfo level)
        {
            return level.Event.SelectMany(x => x.CharacterController).Select(x => x.FaceVoice)
                .Where(x => x.VoiceTrack != null).SelectMany(x => x.VoiceTrack.Files);
        }

        private static IEnumerable<FileInfo> SelectCameraAnimations(LevelFullInfo level)
        {
            return level.Event.Select(x => x.CameraController.CameraAnimation).SelectMany(x => x.Files);
        }

        private static IEnumerable<FileInfo> SelectEventsThumbnails(LevelFullInfo level)
        {
            return level.Event.SelectMany(x => x.Files);
        }

        private static IEnumerable<FileInfo> SelectPhotoAndVideoFiles(LevelFullInfo level)
        {
            var controllers = level.Event.Select(x => x.SetLocationController);
            var photos = controllers.Where(x => x.Photo != null).Select(x => x.Photo).SelectMany(x => x.Files);
            var video = controllers.Where(x => x.VideoClip != null).Select(x => x.VideoClip).SelectMany(x=>x.Files);
            var files = new List<FileInfo>(photos);
            files.AddRange(video);
            return files;
        }

        private async Task SaveFilesToCache(EventFullInfo sent, EventFullInfo resp)
        {
            foreach (var file in sent.Files)
            {
                await SaveFileAsync(resp, file);
            }

            foreach (var file in sent.CameraController.CameraAnimation.Files)
            {
                await SaveFileAsync(resp.CameraController.CameraAnimation, file);
            }

            if (sent.MusicController?.UserSound != null)
            {
                var sentModel = sent.MusicController.UserSound;
                foreach (var file in sentModel.Files)
                {
                    await SaveFileAsync(resp.MusicController.UserSound, file);
                }
            }

            var sentCharacterControllers = sent.CharacterController.OrderBy(x => x.ControllerSequenceNumber).ToArray();
            var respCharacterControllers = resp.CharacterController.OrderBy(x => x.ControllerSequenceNumber).ToArray();
            for (var i = 0; i < sentCharacterControllers.Length; i++)
            {
                await SaveFilesToCache(sentCharacterControllers[i], respCharacterControllers[i]);
            }
        }

        private async Task SaveFilesToCache(CharacterControllerFullInfo sent, CharacterControllerFullInfo resp)
        {
            if (sent.FaceVoice.FaceAnimation != null)
            {
                foreach (var file in sent.FaceVoice.FaceAnimation.Files)
                {
                    await SaveFileAsync(resp.FaceVoice.FaceAnimation, file);
                }
            }

            if (sent.FaceVoice.VoiceTrack != null)
            {
                var sentModel = sent.FaceVoice.VoiceTrack;
                foreach (var file in sentModel.Files)
                {
                    await SaveFileAsync(resp.FaceVoice.VoiceTrack, file);
                }
            }
        }
        
        private async Task<UpdateFilesResult> UpdateEventFiles(string endPoint, Dictionary<long, List<FileInfo>> eventToFiles, Type fileOwnerType)
        {
            var allFiles = eventToFiles.SelectMany(x => x.Value).ToArray();
            var res = await UploadFiles(allFiles);
            if (res.IsError) return new UpdateFilesResult(res.ErrorMessage, res.HttpStatusCode);
            
            var url = ConcatUrl(Host, $"{LevelEndPoint}/{endPoint}");
            var request = RequestHelper.CreateRequest(url, HTTPMethods.Post, true, false);
            var json = Serializer.SerializeToJson(eventToFiles);
            request.AddJsonContent(json);
            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                return new UpdateFilesResult(resp.DataAsText, resp.StatusCode);
            }
            
            var respModel = Serializer.DeserializeJson<Dictionary<long, List<FileInfo>>>(resp.DataAsText);
            foreach (var eventData in respModel)
            {
                var eventId = eventData.Key;
                var updateFiles = eventData.Value;
                foreach (var updateFile in updateFiles)
                {
                    var origin = eventToFiles[eventId].First(x => x.FileType == updateFile.FileType && x.Resolution.Compare(updateFile.Resolution));
                    origin.Version = updateFile.Version;
                    await SaveFileAsync(eventId, fileOwnerType, origin);
                }
            }
            return new UpdateFilesResult(respModel);
        }
    }
}