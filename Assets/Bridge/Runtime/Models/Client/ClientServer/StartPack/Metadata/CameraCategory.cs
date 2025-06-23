using System.Collections.Generic;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class CameraCategory: ICategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<CameraAnimationTemplate> CameraAnimationTemplates { get; set; }
    }
}