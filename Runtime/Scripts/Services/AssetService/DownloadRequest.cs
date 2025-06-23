using System;
using System.Threading;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService
{
    internal abstract class DownloadRequest: IDisposable
    {
        public abstract Object Asset { get; }
        public abstract byte[] AssetBytes { get; }
        public virtual bool IsSuccess { get; protected set; }
        public string ErrorMessage { get; protected set; }
        
        public abstract bool AvailableOnlyRawData { get; }

        public abstract Task DownloadAsset(string url, string token, CancellationToken cancellationToken);

        public virtual void Dispose(){}
    }
}
