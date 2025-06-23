using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;
using Bridge.Services.Advertising;

namespace Bridge
{
    public interface IAdsBridge
    {
        Task<ArrayResult<PromotedSong>> GetPromotedSongs(int take, int skip, CancellationToken token = default);
    }
}