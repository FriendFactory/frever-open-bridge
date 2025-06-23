namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class VideoAssetReader : RawDataReader
    {
        protected override string[] PossibleExtensions => new[] {".mov", ".mp4", ".mpeg"};
    }
}
