using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class UmaBundle : IMainFileContainable, ISizeStorable, IEntity, IStageable
    {
        public List<FileInfo> Files { get; set; }
    }
}