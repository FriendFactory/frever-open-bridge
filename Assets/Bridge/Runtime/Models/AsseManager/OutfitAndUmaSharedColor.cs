namespace Bridge.Models.AsseManager
{
    public partial class OutfitAndUmaSharedColor
    {
        public long OutfitId { get; set; }
        public long UmaSharedColorId { get; set; }
        public int Color { get; set; }
        
        public virtual Outfit Outfit { get; set; }
        public virtual UmaSharedColor UmaSharedColor { get; set; }
    }
}