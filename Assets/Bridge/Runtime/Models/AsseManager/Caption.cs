namespace Bridge.Models.AsseManager
{
    public partial class Caption
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public int ControllerSequenceNumber { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int RotationDegrees { get; set; }
        public long FontId { get; set; }
        public int FontSize { get; set; }
        public string Text { get; set; }
        public int TextColor { get; set; }
        public int BackgroundColor { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual Font Font { get; set; }
    }
}