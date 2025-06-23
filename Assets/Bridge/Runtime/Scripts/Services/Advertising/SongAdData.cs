using System;

namespace Bridge.Services.Advertising
{
    [Serializable]
    public sealed class SongAdData
    {
        public long SongId { get; set; }
        public string BannerUrl { get; set; }

        private bool Equals(SongAdData other)
        {
            return SongId == other.SongId && BannerUrl == other.BannerUrl;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is SongAdData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SongId.GetHashCode() * 397) ^ (BannerUrl != null ? BannerUrl.GetHashCode() : 0);
            }
        }
    }
}