using System;

namespace Bridge.Services.AssetService.Caching
{
    public sealed class FileData
    {
        public string Path { get; set; }
        public string Version { get; set; }
        public DateTime DownloadedDateUTC { get; set; }
        public DateTime LastUsedDateUTC { get; set; }
        public int UsingCount { get; set; }
        public string AssetTypeName { get; set; }
        public long AssetId { get; set; }
        public long SizeKb { get; set; }
        
        public FileData(){}

        public FileData(string version, string path, 
            DateTime downloadedDateUTC, DateTime lastUsedDateUTC,
            int usingCount, string assetTypeName, long assetId, long sizeKb)
        {
            Version = version;
            Path = path;
            DownloadedDateUTC = downloadedDateUTC;
            LastUsedDateUTC = lastUsedDateUTC;
            UsingCount = usingCount;
            AssetTypeName = assetTypeName;
            AssetId = assetId;
            SizeKb = sizeKb;
        }
    }
}