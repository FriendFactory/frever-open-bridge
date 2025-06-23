using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class EventFullInfo: IEventInfo
    {
        public int TargetCharacterSequenceNumber { get; set; }

        public long CharacterSpawnPositionId { get; set; }

        public long CharacterSpawnPositionFormationId { get; set; }

        public int Length { get; set; }

        public long? TemplateId { get; set; }

        public int LevelSequence { get; set; }

        public long Id { get; set; }

        public bool HasActualThumbnail { get; set; }

        public List<FileInfo> Files { get; set; }

        public List<CharacterControllerFullInfo> CharacterController { get; set; }

        public CameraControllerFullInfo CameraController { get; set; }

        public SetLocationControllerFullInfo SetLocationController { get; set; }

        public VfxControllerFullInfo VfxController { get; set; }

        public CameraFilterControllerFullInfo CameraFilterController { get; set; }
        
        public MusicControllerFullInfo MusicController { get; set; }

        public List<CaptionFullInfo> Caption { get; set; }
    }
}