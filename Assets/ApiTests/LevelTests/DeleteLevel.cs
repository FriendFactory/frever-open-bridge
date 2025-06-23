using Bridge.Models.AsseManager;

namespace ApiTests.Levels
{
    public class DeleteLevel: EntityApiTest<Level>
    {
        protected override async void RunTestAsync()
        {
            var anyLevelId = await GetAnyAvailableEntityId<Level>();
            var deleteRes = await Bridge.DeleteAsync<Level>(anyLevelId);
            LogResult(deleteRes);
        }
    }
}