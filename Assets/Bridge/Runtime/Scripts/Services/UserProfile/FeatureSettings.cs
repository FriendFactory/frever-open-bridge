using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Services.UserProfile
{
    public class FeatureSettings
    {
        public bool AllowCreatingNewLevel { get; set; }
        
        public int RequiredVideoCount { get; set; }
        
        public int CurrentVideoCount { get; set; }
        
        [ProtoNewField(1)] public Settings VideoToFeed { get; set; }
        [ProtoNewField(2)] public Settings CrewCreation { get; set; }
        [ProtoNewField(3)] public Settings TemplateFromVideoCreation { get; set; }
        [ProtoNewField(4)] public Settings VideoStyleTransformation { get; set; }
    }
    
    public class Settings
    {
        public bool AllowFeature { get; set; }
        public int CurrentValue { get; set; }
        public int RequiredValue { get; set; }
    }
}