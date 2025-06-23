using Bridge.Models.ClientServer.StartPack.Metadata;

namespace Bridge.Models.ClientServer.StartPack
{
    public sealed class WardrobeCategoriesForGender
    {
        public long GenderId { get; set; }
        public WardrobeCategory[] WardrobeCategories { get; set; }
    }
}