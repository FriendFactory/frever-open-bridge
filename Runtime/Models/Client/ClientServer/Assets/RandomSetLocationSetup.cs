namespace Bridge.Models.ClientServer.Assets
{
    public class RandomSetLocationSetup
    {
        public int? RequestedCharacterCount { get; set; }

        public int SupportedCharacterCount { get; set; }

        public SetLocationFullInfo SetLocation { get; set; }

        public CharacterPosition[] CharacterPositions { get; set; }
    }

    public class CharacterPosition
    {
        public CharacterSpawnPositionInfo CharacterSpawnPosition { get; set; }

        public BodyAnimationInfo BodyAnimation { get; set; }
    }
}