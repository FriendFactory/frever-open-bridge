using Bridge.Results;
using UnityEngine;

namespace Bridge
{
    public sealed class BannerTextureResult: Result
    {
        public readonly Texture2D Banner;

        internal BannerTextureResult(Texture2D banner)
        {
            Banner = banner;
        }

        internal BannerTextureResult(string error):base(error)
        {
        }   
    }
}