using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class CameraFilterSubCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CameraFilterCategoryId { get; set; }

        public virtual CameraFilterCategory CameraFilterCategory { get; set; }
        public virtual ICollection<CameraFilter> CameraFilter { get; set; }

        public CameraFilterSubCategory()
        {
            CameraFilter = new HashSet<CameraFilter>();
        }
    }
}