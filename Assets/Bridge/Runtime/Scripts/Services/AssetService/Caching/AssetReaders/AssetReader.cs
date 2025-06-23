using System;
using System.Threading;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal abstract class AssetReader
    {
        public Object Asset { get; protected set; }
        public byte[] RawData { get; protected set; }
        public abstract bool ProvidesUnityObject { get; }

        protected abstract string[] PossibleExtensions { get; }
        
        public bool CanHandle(string extension)
        {
            foreach (var extensions in PossibleExtensions)
            {
                if (extensions.Equals(extension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public abstract Task Read(string path, CancellationToken cancellationToken);
    }
}