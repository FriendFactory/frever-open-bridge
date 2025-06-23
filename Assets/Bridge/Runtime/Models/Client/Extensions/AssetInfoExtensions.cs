using System;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common;

namespace Bridge.Models.Extensions
{
    internal static class AssetInfoExtensions 
    {
        public static IFilesAttachedEntity ToAssetModel(this AssetInfo assetInfo)
        {
            switch (assetInfo.AssetType)
            {
                case AssetStoreAssetType.Wardrobe:
                    return ApplyAssetInfo(new WardrobeShortInfo(), assetInfo);
                case AssetStoreAssetType.SetLocation:
                    return ApplyAssetInfo(new SetLocationFullInfo(), assetInfo);
                case AssetStoreAssetType.CameraFilter:
                    return ApplyAssetInfo(new CameraFilterInfo(), assetInfo);
                case AssetStoreAssetType.Vfx:
                    return ApplyAssetInfo(new VfxInfo(), assetInfo);
                case AssetStoreAssetType.VoiceFilter:
                    return ApplyAssetInfo(new VoiceFilterFullInfo(), assetInfo);
                case AssetStoreAssetType.BodyAnimation:
                    return ApplyAssetInfo(new BodyAnimationInfo(), assetInfo);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IFilesAttachedEntity ApplyAssetInfo(IFilesAttachedEntity target, AssetInfo assetInfo)
        {
            target.Id = assetInfo.Id;
            target.Files = assetInfo.Files;
            return target;
        }
    }
}
