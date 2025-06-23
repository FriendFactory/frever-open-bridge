using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.AdminService;
using Bridge.Models.ClientServer.Assets;
using Bridge.Results;

namespace Bridge
{
    public interface ICharacterBakingBridge
    {
        public Task<ArrayResult<CharacterFullInfo>> GetNonBakedCharacters(int count, CancellationToken token = default);
        public Task<ArrayResult<CharacterFullInfo>> GetCharactersAdminAccessLevel(long[] ids, CancellationToken token = default);
        Task<Result> InvalidateWardrobe(long wardrobeId, string reason);
        Task<Result> UploadBakedView(CharacterBakedViewDto bakedView);
        Task<Result> UpdateBakedView(long id, CharacterBakedViewDto bakedView);

    }
}