using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Exceptions;
using Bridge.Services.AssetService.Caching.Encryption;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal abstract class EncryptedUnityAssetReader: UnityAssetReader
    {
        protected readonly IEncryptedFileReader _encryptedFileReader;
        
        internal EncryptedUnityAssetReader(IEncryptedFileReader encryptedFileReader)
        {
            _encryptedFileReader = encryptedFileReader;
        }

        public override Task Read(string path, CancellationToken cancellationToken)
        {
            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Could not find file # {path}");
                }
            
                return ReadInternal(path, cancellationToken);
            }
            catch (Exception e)
            {
                throw new FileEncryptionException($"Failed to read asset: {e.Message}", e);
            }
        }

        protected abstract Task ReadInternal(string path, CancellationToken cancellationToken);
        
        protected override UnityWebRequest CreateRequest(string path)
        {
            return UnityWebRequestAssetBundle.GetAssetBundle(path);
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request) 
        {
            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            return bundle;
        }
    }
}