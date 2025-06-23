using Bridge.Models.AsseManager;

namespace ApiTests.CharactersTests
{
    public class DeleteCharacterById: EntityApiTest<Character>
    {
        public long Id;

        protected override async void RunTestAsync()
        {
            var deleteResult = await Bridge.DeleteAsync<Character>(Id);
            LogResult(deleteResult);
        }
    }
}