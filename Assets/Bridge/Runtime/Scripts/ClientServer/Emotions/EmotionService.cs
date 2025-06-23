using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Emotions
{
    internal interface IEmotionService
    {
        Task<ArrayResult<Emotion>> GetEmotionsAsync(int take, int skip, CancellationToken token);
        Task<Result<EmotionAssetsSetup>> GetEmotionAssetsSetupAsync(long emotionId, CancellationToken token);
    }

    internal sealed class EmotionService : ServiceBase, IEmotionService
    {
        private const string END_POINT = "emotion";
        
        public EmotionService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<Emotion>> GetEmotionsAsync(int take, int skip, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}?&take={take}&skip={skip}");
                return await SendRequestForListModels<Emotion>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ArrayResult<Emotion>.Cancelled() : ArrayResult<Emotion>.Error(e.Message);
            }
        }

        public async Task<Result<EmotionAssetsSetup>> GetEmotionAssetsSetupAsync(long emotionId, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"{END_POINT}/{emotionId}/assets");
                return await SendRequestForSingleModel<EmotionAssetsSetup>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? Result<EmotionAssetsSetup>.Cancelled() : Result<EmotionAssetsSetup>.Error(e.Message);
            }
        }
    }
}