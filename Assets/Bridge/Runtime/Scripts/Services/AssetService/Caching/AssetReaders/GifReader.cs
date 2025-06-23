namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class GifReader: RawDataReader
    {
        protected override string[] PossibleExtensions => new[] {".gif"};
    }
}