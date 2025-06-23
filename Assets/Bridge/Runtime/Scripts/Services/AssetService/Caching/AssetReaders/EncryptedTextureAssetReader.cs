using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Services.AssetService.Caching.Encryption;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class EncryptedTextureAssetReader: EncryptedUnityAssetReader
    {
        protected override string[] PossibleExtensions { get; }

        public EncryptedTextureAssetReader(IEncryptedFileReader encryptedFileReader) : base(encryptedFileReader)
        {
            PossibleExtensions = new[] { ".jpg", ".jpeg", ".png" }
                .Select(extension => $"{extension}{Encryption.Constants.ENCRYPTED_FILE_EXTENSION}")
                .ToArray();
        }

        protected override async Task ReadInternal(string path, CancellationToken cancellationToken)
        {
            var imageData = await _encryptedFileReader.DecryptFileToMemoryAsync(path, cancellationToken);
            // texture size does not matter, since LoadImage will replace with incoming image size.
            var texture = new Texture2D(42, 42);

            texture.LoadImage(imageData);

            Asset = texture;
            Asset.name = Path.GetFileName(path);;
        }
    }
}