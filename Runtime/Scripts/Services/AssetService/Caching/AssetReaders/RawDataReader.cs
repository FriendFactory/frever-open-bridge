using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal abstract class RawDataReader: AssetReader
    {
        public override bool ProvidesUnityObject => false;

        public override async Task Read(string path, CancellationToken cancellationToken)
        {
            RawData = await Task.Run(() => File.ReadAllBytes(path), cancellationToken);
        }
    }
}