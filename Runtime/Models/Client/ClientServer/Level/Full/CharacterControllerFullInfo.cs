namespace Bridge.Models.ClientServer.Level.Full
{
    public class CharacterControllerFullInfo
    {
        public long Id { get; set; }

        public long CharacterId { get; set; }
        
        public int ControllerSequenceNumber { get; set; }

        public long? OutfitId { get; set; }
        
        public long? CharacterSpawnPositionId { get; set; }

        public CharacterControllerBodyAnimationFullInfo BodyAnimation { get; set; }

        public CharacterControllerFaceVoiceFullInfo FaceVoice { get; set; }
    }
}