using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CameraAnimationFullInfo: IMainFileContainable
    {
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }
        
        [ProtoNewField(1)] public CameraAnimationFrameInfo FirstFrame { get; set; }
        [ProtoNewField(2)] public CameraAnimationFrameInfo LastFrame { get; set; }
        [ProtoNewField(3)] public long CameraAnimationTemplateId { get; set; }
    }
}