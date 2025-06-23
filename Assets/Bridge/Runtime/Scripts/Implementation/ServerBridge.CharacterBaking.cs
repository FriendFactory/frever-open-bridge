using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.AdminService;
using Bridge.Models.ClientServer.Assets;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        public Task<ArrayResult<CharacterFullInfo>> GetNonBakedCharacters(int count, CancellationToken token = default)
        {
            return _characterBakingService.GetNonBakedCharacters(count, token);
        }

        public Task<ArrayResult<CharacterFullInfo>> GetCharactersAdminAccessLevel(long[] ids, CancellationToken token = default)
        {
            return _characterBakingService.GetCharactersAdminAccessLevel(ids, token);
        }

        public Task<Result> InvalidateWardrobe(long wardrobeId, string reason)
        {
            return _characterBakingService.InvalidateWardrobe(wardrobeId, reason);
        }

        public Task<Result> UploadBakedView(CharacterBakedViewDto bakedView)
        {
            return _characterBakingService.UploadBakedView(bakedView);
        }

        public Task<Result> UpdateBakedView(long id, CharacterBakedViewDto bakedView)
        {
            return _characterBakingService.UpdateBakedView(id, bakedView);
        }
    }
}