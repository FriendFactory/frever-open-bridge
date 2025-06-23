using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.Services.Advertising
{
    internal interface IAdvertisingService
    {
        Task<ArrayResult<PromotedSong>> GetPromotedSongs(int take, int skip, CancellationToken token);
    }
}
