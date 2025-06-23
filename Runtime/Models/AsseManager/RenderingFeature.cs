using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class RenderingFeature
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public RenderingFeature()
        {
            CameraFilterVariantAndRendererFeature = new List<CameraFilterVariantAndRenderingFeature>();
        }

        public virtual ICollection<CameraFilterVariantAndRenderingFeature> CameraFilterVariantAndRendererFeature { get; set; }
    }
}