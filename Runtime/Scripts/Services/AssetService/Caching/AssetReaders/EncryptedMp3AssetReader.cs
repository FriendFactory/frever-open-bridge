using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Services.AssetService.Caching.Encryption;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class EncryptedMp3AssetReader : EncryptedUnityAssetReader 
    {
        protected override string[] PossibleExtensions { get; }

        public EncryptedMp3AssetReader(IEncryptedFileReader encryptedFileReader) : base(encryptedFileReader)
        {
            PossibleExtensions = new[] { $".mp3{Encryption.Constants.ENCRYPTED_FILE_EXTENSION}" };
        }

        protected override async Task ReadInternal(string path, CancellationToken cancellationToken)
        {
            var audioData = await _encryptedFileReader.DecryptFileToMemoryAsync(path, cancellationToken);
            // it is not possible to decode and convert mp3 to AudioClip w/o external plugins,
            // so, the trick is to make an unencrypted copy and read it using UnityWebRequest
            var tempPath = path.Remove(path.Length - Encryption.Constants.ENCRYPTED_FILE_EXTENSION.Length);
            using (var tempFile = File.OpenWrite(tempPath))
            using (var dataStream = new MemoryStream(audioData))
            {
                await dataStream.CopyToAsync(tempFile);
            }
            
            Asset = await ReadUnityObjectFromFile(tempPath, cancellationToken);
            Asset.name = Path.GetFileName(path);
            
            File.Delete(tempPath);
        }
        
        protected override UnityWebRequest CreateRequest(string path)
        {
            var wr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
            ((DownloadHandlerAudioClip)wr.downloadHandler).streamAudio = false;
            return wr;
        }

        protected override Object ExtractDownloadedFile(UnityWebRequest request)
        {
            return DownloadHandlerAudioClip.GetContent(request);
        }
        
        private async Task<Object> ReadUnityObjectFromFile(string path, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("Path is null");
            }

            using var request = CreateRequest($"file://{path}");
            await request.SendWebRequest();
                
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception($"Failed read file from disk. Error: {request.error}");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var file = ExtractDownloadedFile(request);
            file.name = Path.GetFileName(path);

            return file;
        }
    }
}