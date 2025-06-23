using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Other
{
    internal interface IRandomAssetsService
    {
        Task<Result<RandomSetLocationSetup>> GetRandomSetLocationSetup(int characterCount, CancellationToken token);
    }

    internal sealed class RandomAssetsService: AssetServiceBase, IRandomAssetsService
    {
        public RandomAssetsService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<Result<RandomSetLocationSetup>> GetRandomSetLocationSetup(int characterCount, CancellationToken token)
        {
            try
            {
                return await GetRandomSetLocationSetupInternal(characterCount, token);
            }
            catch (OperationCanceledException)
            {
                return Result<RandomSetLocationSetup>.Cancelled();
            }
        }

        private Task<Result<RandomSetLocationSetup>> GetRandomSetLocationSetupInternal(int characterCount, CancellationToken token)
        {
            var url = BuildUrl($"random-scene?characterNumber={characterCount}");
            return SendRequestForSingleModel<RandomSetLocationSetup>(url, token);
        }
    }
}