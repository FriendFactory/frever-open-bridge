using Bridge.Models.AsseManager;
using UnityEngine;

namespace ApiTests.CharacterSpawnPositionTests
{
    public class DeleteCharacterSpawnPosition : EntityApiTest<CharacterSpawnPosition>
    {
        protected override async void RunTestAsync()
        {
            var anySpawnPosition = await GetAnyAvailableEntityId<CharacterSpawnPosition>();
            Debug.Log(anySpawnPosition);
            var deleteResult = await Bridge.DeleteAsync<CharacterSpawnPosition>(anySpawnPosition);
            LogResult(deleteResult);
        }
    }
}
