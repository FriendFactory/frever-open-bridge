using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal class WardrobeCleaner: GenericCleaner<Wardrobe>
    {
        protected override Type[] AllowedTypesToCreate => new[]
        {
            typeof(UmaBundle), typeof(UmaAsset),
            typeof(UmaAssetFile), typeof(UmaBundleAllDependency), typeof(UmaBundleDirectDependency),
            typeof(UmaAssetFileAndUnityAssetType), typeof(WardrobeAndWardrobeSubCategory), typeof(WardrobeGenderGroup),
            typeof(Wardrobe)//inside gender group
        };
    }
}