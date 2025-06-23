using System.Linq;
using Bridge.Models.Common.Files;

namespace ApiTests.MovementTypeTests
{
    internal sealed class FetchMovementTypeThumbnails : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var metaPack = await Bridge.GetMetadataStartPackAsync();
            var movementType = metaPack.Pack.MovementTypes.First(x => x.Id == 1);
            var resp = await Bridge.FetchThumbnailAsync(movementType, Resolution._128x128);
        }
    }
}
