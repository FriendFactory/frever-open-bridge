using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.Common
{
    public interface IFilesAttachedEntity: IEntity, IFilesAttached
    {
        
    }

    public interface IFilesAttached
    {
        List<FileInfo> Files { get; set; }
    }
}
