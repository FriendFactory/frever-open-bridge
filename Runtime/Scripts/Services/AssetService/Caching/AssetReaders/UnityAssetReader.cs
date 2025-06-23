using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ExternalPackages.AsynAwaitUtility;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal abstract class UnityAssetReader: AssetReader
    {
        public sealed override bool ProvidesUnityObject => true;

        public override async Task Read(string path, CancellationToken cancellationToken)
        {
            Asset = (Object) await ReadUnityObjectFromFile(path, cancellationToken);
        }

        private IEnumerator ReadUnityObjectFromFile(string path, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("Path is null");
            }

            using var request = CreateRequest($"file://{path}");
            yield return request.SendWebRequest();
                
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception($"Failed read file from disk. Error: {request.error}");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var file = ExtractDownloadedFile(request);
            OnAssetExtracted(file, cancellationToken);
            file.name = Path.GetFileName(path);
            yield return file;
        }

        protected abstract UnityWebRequest CreateRequest(string path);
        protected abstract Object ExtractDownloadedFile(UnityWebRequest request);
        protected virtual void OnAssetExtracted(Object asset, CancellationToken token){}
    }
}