using ApiTests;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;

public class GetOutfitTests : EntityApiTest<Outfit>
{
    protected override async void RunTestAsync()
    {
        var req = new Query<Outfit>();
        await Bridge.GetAsync(req);
    }
}
