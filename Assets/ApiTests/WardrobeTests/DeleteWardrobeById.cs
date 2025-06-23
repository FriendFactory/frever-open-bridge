using Bridge.Models.AsseManager;

namespace ApiTests.WardorbeTests
{
    public sealed class DeleteWardrobeById: EntityApiTest<Wardrobe>
    {
        public long Id;

        protected override async void RunTestAsync()
        {
            var resp = await Bridge.DeleteAsync<Wardrobe>(Id);
            LogResult(resp);
        }
    }
}