using System.Linq;

namespace ApiTests.VideoTests
{
    public sealed class DownloadWatermarkTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var metadataResp = await Bridge.GetMetadataStartPackAsync();
            var ips = metadataResp.Pack.IntellectualProperty;
            var watermark = ips.First(x => x.Watermark != null).Watermark;
            var downloadRes = await Bridge.FetchMainAssetAsync(watermark, watermark.Files.First());
            downloadRes = await Bridge.FetchMainAssetAsync(watermark, watermark.Files.Last());
        }
    }
}