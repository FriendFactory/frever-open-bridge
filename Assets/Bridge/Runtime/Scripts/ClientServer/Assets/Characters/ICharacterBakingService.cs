using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.AdminService;
using Bridge.Models.ClientServer.Assets;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Bridge.Services.AssetService;

namespace Bridge.ClientServer.Assets.Characters
{
    internal interface ICharacterBakingService
    {
        Task<ArrayResult<CharacterFullInfo>> GetNonBakedCharacters(int count, CancellationToken token = default);
        Task<ArrayResult<CharacterFullInfo>> GetCharactersAdminAccessLevel(long[] ids, CancellationToken token = default);
        Task<Result> InvalidateWardrobe(long wardrobeId, string reason);
        Task<Result> UploadBakedView(CharacterBakedViewDto bakedView);
        Task<Result> UpdateBakedView(long id, CharacterBakedViewDto bakedView);
    }

    internal sealed class CharacterBakingService: ServiceBase, ICharacterBakingService
    {
        private readonly IAssetService _assetService;
        
        public CharacterBakingService(string host, IRequestHelper requestHelper, ISerializer serializer, IAssetService assetService) : base(host, requestHelper, serializer)
        {
            _assetService = assetService;
        }
        
        public async Task<ArrayResult<CharacterFullInfo>> GetNonBakedCharacters(int count, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, $"baking/non-baked?count={count}");
                return await SendRequestForListModels<CharacterFullInfo>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ArrayResult<CharacterFullInfo>.Cancelled() : ArrayResult<CharacterFullInfo>.Error(e.Message);
            }
        }

        public async Task<ArrayResult<CharacterFullInfo>> GetCharactersAdminAccessLevel(long[] ids, CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, "baking/characters");
                return await SendRequestForListModels<CharacterFullInfo>(url, token, ids);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException ? ArrayResult<CharacterFullInfo>.Cancelled() : ArrayResult<CharacterFullInfo>.Error(e.Message);
            }
        }

        public async Task<Result> InvalidateWardrobe(long wardrobeId, string reason)
        {
            try
            {
                var url = ConcatUrl(Host, "asset/moderation/wardrobe/baking/availability");
                var body = new
                {
                    WardrobeId = wardrobeId,
                    Reason = reason
                };
                return await SendPostRequest(url, body);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> UploadBakedView(CharacterBakedViewDto bakedView)
        {
            try
            {
                var filesToDeploy = bakedView.Files;
                var resp = await _assetService.UploadFilesAsync(filesToDeploy.ToArray(), default);
                if (resp.Any(x => !x.IsSuccess))
                    return resp.First(x => !x.IsSuccess);

                foreach (var fileInfo in filesToDeploy)
                {
                    fileInfo.TagAsDeployed();
                }
                var url = ConcatUrl(Host, "baking");
                return await SendPostRequest(url, bakedView);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }

        public async Task<Result> UpdateBakedView(long id, CharacterBakedViewDto bakedView)
        {
            try
            {
                var filesToDeploy = bakedView.Files;
                var resp = await _assetService.UploadFilesAsync(filesToDeploy.ToArray(), default);
                if (resp.Any(x => !x.IsSuccess))
                    return resp.First(x => !x.IsSuccess);

                foreach (var fileInfo in filesToDeploy)
                {
                    fileInfo.TagAsDeployed();
                }
                var url = ConcatUrl(Host, $"baking/{id}");
                return await SendPatchRequest(url, bakedView);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
        }
    }
}