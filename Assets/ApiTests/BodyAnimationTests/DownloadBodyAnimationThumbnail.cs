using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using BodyAnimation = Bridge.Models.AsseManager.BodyAnimation;

namespace ApiTests.BodyAnimationTests
{
    internal sealed class DownloadBodyAnimationThumbnail : EntityApiTest<BodyAnimation>
    {
        protected override async void RunTestAsync()
        {
            var id = await GetAnyAvailableEntityId<BodyAnimation>();
            var model = await Bridge.GetAsync<BodyAnimation>(id);
            var resp = await Bridge.GetThumbnailAsync(model.ResultObject, Resolution._256x256);
        }
    }
}
