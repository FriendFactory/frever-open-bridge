namespace Bridge.Results
{
    public sealed class CanceledGetAssetResult: GetAssetResult
    {
        internal CanceledGetAssetResult() : base(true)
        {
        }
    }
}