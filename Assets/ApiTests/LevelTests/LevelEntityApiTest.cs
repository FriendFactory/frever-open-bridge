using System.Linq;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;

namespace ApiTests.Levels
{
    public abstract class LevelEntityApiTest: EntityApiTest<Level>
    {
        protected async Task<long> GetAnySpawnPositionId(long setLocationId)
        {
            var query = new Query<SetLocation>();
            query.SetFilters(new FilterSetup()
            {
                FieldName = nameof(SetLocation.Id),
                FilterValue = setLocationId,
                FilterType = FilterType.Equals
            });

            query.ExpandField(nameof(SetLocation.SetLocationAndCharacterSpawnPosition));

            var res = await Bridge.GetAsync(query);
            return res.Models.First().SetLocationAndCharacterSpawnPosition.First().CharacterSpawnPositionId;
        }
    }
}