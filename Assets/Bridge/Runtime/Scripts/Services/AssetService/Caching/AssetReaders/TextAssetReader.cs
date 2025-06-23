using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching.AssetReaders
{
    internal sealed class TextAssetReader : AssetReader
    {
        public override bool ProvidesUnityObject => true;
        protected override string[] PossibleExtensions => new[]{".txt"};
        public override async Task Read(string path, CancellationToken cancellationToken)
        {
            var text = await Task.Run(()=> File.ReadAllText(path), cancellationToken);
            Asset = new TextAsset(text);
        }
    }
}
