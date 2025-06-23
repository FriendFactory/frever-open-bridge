using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Services.AssetService.Caching.Encryption;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class EncryptedAssetBundleReader : EncryptedUnityAssetReader
    {
        protected override string[] PossibleExtensions { get; }

        public EncryptedAssetBundleReader(IEncryptedFileReader encryptedFileReader) : base(encryptedFileReader)
        {
            PossibleExtensions = new[] { Encryption.Constants.ENCRYPTED_FILE_EXTENSION };
        }

        protected override async Task ReadInternal(string path, CancellationToken cancellationToken)
        {
            var bundleData = await _encryptedFileReader.DecryptFileToMemoryAsync(path, cancellationToken);
            
            Asset = await AssetBundle.LoadFromMemoryAsync(bundleData);
            Asset.name = Path.GetFileName(path);
        }
    }
}