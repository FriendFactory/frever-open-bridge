using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.Common;

namespace ApiTests.SetLocationBundleTests
{
    public class UpdateCharacterSpawnPositionInSetLocationBundle : EntityApiTest<SetLocationBundle>
    {
        protected override async void RunTestAsync()
        {
            var anyBundleId = await GetAnyAvailableEntityId<SetLocationBundle>();

            var query = new Query<SetLocationBundle>();
            query.ExpandField(nameof(SetLocationBundle.CharacterSpawnPosition));
            query.SetFilters(new FilterSetup(nameof(IEntity.Id), FilterType.Equals, anyBundleId));

            var res = await Bridge.GetAsync(query);
            var setLocationBundle = res.Models.First();
            setLocationBundle.CharacterSpawnPosition.First().SpawnPositionSpaceSizeId = 3;

            var updateResult = await Bridge.UpdateAsync(setLocationBundle);

            LogResult(updateResult);
        }
    }
}