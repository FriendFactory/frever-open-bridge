using System.Linq;
using Bridge.Services.AssetService.Caching.Encryption;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class AssetReaderProvider
    {
        private readonly AssetReader[] _readers;

        internal AssetReaderProvider(EncryptionService encryptionService)
        {
            var encryptedReader = encryptionService.GetEncryptedFileReader();
            var partialEncryptedReader = encryptionService.GetPartialEncryptedFileReader();
            
            _readers = new AssetReader[]
            {
                new TextAssetReader(),
                new VideoAssetReader(),
                new AssetBundleReader(),
                new WavAssetReader(),
                new OggAssetReader(),
                new Mp3AssetReader(),
                new TextureAssetReader(),
                new GifReader(),
                new EncryptedWavAssetReader(encryptedReader), 
                new EncryptedTextureAssetReader(encryptedReader), 
                new EncryptedMp3AssetReader(encryptedReader), 
                new EncryptedAssetBundleReader(partialEncryptedReader),
            };
        }

        public AssetReader GetReader(string fileExtension)
        {
            return _readers.FirstOrDefault(x=>x.CanHandle(fileExtension));
        }
    }
}
