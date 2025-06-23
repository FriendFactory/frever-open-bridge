namespace Bridge.Models.AsseManager
{
    public class CameraFilterVariantAndRenderingFeature
    {
        public long CameraFilterVariantId { get; set; }
        public long RenderingFeatureId { get; set; }
        
        public virtual CameraFilterVariant CameraFilterVariant { get; set; }
        public virtual RenderingFeature RenderingFeature { get; set; }
    }
}