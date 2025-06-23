namespace Bridge.Models.ClientServer.EditorsSetting
{
    public sealed class CharacterEditorSettings: IEditorSettings<ICharacterEditorSetting>
    {
        public ICharacterEditorSetting[] Settings => this.AllSettings();
        public WardrobeCategorySettings[] WardrobeCategories { get; set; }
        public FilterSettings FilterSettings { get; set; }
        public SavingCharacterSettings SavingCharacterSettings { get; set; }
        public SavingOutfitSettings SavingOutfitSettings { get; set; }
        public ZoomSettings ZoomSettings { get; set; }
    }

    public sealed class WardrobeCategorySettings
    {
        public long Id { get; set; }
        public WardrobeSubCategorySettings[] Subcategories { get; set; }
    }

    public sealed class WardrobeSubCategorySettings
    {
        public long Id { get; set; }
        public bool AllowAdjustment { get; set; }
    }

    public sealed class FilterSettings
    {
        public bool AllowFiltering { get; set; }
    }

    public sealed class SavingCharacterSettings
    {
        public bool AllowSaveCharacter { get; set; }
    }
    
    public sealed class SavingOutfitSettings
    {
        public bool AllowSaveOutfit { get; set; }
    }

    public sealed class ZoomSettings
    {
        public bool AllowZooming { get; set; }
    }
}