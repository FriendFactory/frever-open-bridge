using System;

namespace Bridge.Models.Common
{
    public interface ITimeChangesTrackable
    {
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
    }
}