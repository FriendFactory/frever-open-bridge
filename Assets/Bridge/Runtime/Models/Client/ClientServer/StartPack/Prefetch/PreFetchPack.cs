using System.Collections;
using System.Collections.Generic;
using Bridge.Models.AsseManager;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Prefetch
{
    public sealed class PreFetchPack: IStartPack
    {
        public List<PreFetchEntityInfo> BodyAnimations { get; set; }

        public List<PreFetchEntityInfo> CameraFilterVariants { get; set; }

        public List<PreFetchEntityInfo> SetLocationBundles { get; set; }

        public List<PreFetchEntityInfo> Vfxs { get; set; }

        public List<PreFetchEntityInfo> UmaBundles { get; set; }
    }
}