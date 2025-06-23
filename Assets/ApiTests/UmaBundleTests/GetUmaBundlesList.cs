using ApiTests;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;

public class GetUmaBundlesList : EntityApiTest<UmaBundle>
{
    protected override async void RunTestAsync()
    {
        var umaExpandFields = new []
        {
            new []
            {
                nameof(UmaBundle.UmaAsset),
                nameof(UmaAsset.UmaAssetFile),
                nameof(UmaAssetFile.UmaAssetFileAndUnityAssetType),
                nameof(UmaAssetFileAndUnityAssetType.UnityAssetType)
            },
            new []
            {
                nameof(UmaBundle.UmaAsset),
                nameof(UmaAsset.AssetWardrobeSlot)
            },
            new []
            {
                nameof(UmaBundle.UmaBundleType)
            },
            new []
            {
                nameof(UmaBundle.UmaBundleAllDependencyUmaBundle),
                nameof(UmaBundleAllDependency.DependsOnBundle)
            }
        };
        
        var q = new Query<UmaBundle>();
        foreach(var field in umaExpandFields)
        {
            var exp = q.ExpandField(field[0]);

            for (var i = 1; i < field.Length; i++)
            {
                exp.ThenExpand(field[i]);
            }
        }

        var resp = await Bridge.GetAsync(q);
    }
}
