namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class CaptionMetadata
    {
        public float MaxFontSize { get; set; }

        public float MinFontSize { get; set; }

        public float DefaultFontSize { get; set; }

        public int MaxCaptionCount { get; set; }

        public int MaxCharacterCount { get; set; }

        public CaptionColor[] Colors { get; set; }

        public CaptionFont[] Fonts { get; set; }
    }
    
    public class CaptionColor
    {
        public ColorPair[] ColorPairs { get; set; }
    }

    public class ColorPair
    {
        public string EffectVariant { get; set; }

        public string Font { get; set; }

        public string Background { get; set; }
    }

    public class CaptionFont
    {
        public long Id { get; set; }

        public string FontKey { get; set; }

        public string Name { get; set; }
    }
}