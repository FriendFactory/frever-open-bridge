using System;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class CaptionFullInfo: IEntity
    {
        public long Id { get; set; }
        public long FontId { get; set; }
        public string Text { get; set; }
        
        //todo: uncomment Obsolete attributes when we have deployed the server with new field on Stage/Prod
        //[Obsolete("Use X")] 
        public int PositionX { get; set; }
        //[Obsolete("Use Y")] 
        public int PositionY { get; set; }
        //[Obsolete("Use Rotation")] 
        public int RotationDegrees { get; set; }
        //[Obsolete("Use FontSizeUnits")] 
        public int FontSize { get; set; }
        //[Obsolete("Use TextColorRgb")] 
        public int TextColor { get; set; }
        //[Obsolete("Use BackgroundColorRgb")] 
        public int BackgroundColor { get; set; }
        
        [ProtoNewField(1)] public CaptionTextAlignment TextAlignment { get; set; }
        [ProtoNewField(2)] public CaptionSettings Settings { get; set; }
        [ProtoNewField(3)] public float X { get; set; }
        [ProtoNewField(4)] public float Y { get; set; }
        [ProtoNewField(5)] public float Rotation { get; set; }
        [ProtoNewField(6)] public float FontSizeUnits { get; set; }
        [ProtoNewField(7)] public string TextColorRgb { get; set; }
        [ProtoNewField(8)] public string BackgroundColorRgb { get; set; }
    }
    
    public enum CaptionTextAlignment
    {
        Left,
        Center,
        Right
    }

    public class CaptionSettings
    {
        public int BackgroundType { get; set; }

        public string EffectVariant { get; set; }
    }
}