using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class CameraFilterVariant
    {
        public CameraFilterVariant()
        {
            CameraFilterVariantAndRendererFeature = new HashSet<CameraFilterVariantAndRenderingFeature>();
            CameraFilterController = new HashSet<CameraFilterController>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long CameraFilterId { get; set; }
        public int SortOrder { get; set; }
        public long SizeKb { get; set; }

        public long ReadinessId { get; set; }
        public string FilesInfo { get; set; }
        
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        
        public virtual CameraFilter CameraFilter { get; set; }
        public virtual Readiness Readiness { get; set; }
        
        public virtual ICollection<CameraFilterVariantAndRenderingFeature> CameraFilterVariantAndRendererFeature { get; set; }
        public virtual ICollection<CameraFilterController> CameraFilterController { get; set; }
    }
}