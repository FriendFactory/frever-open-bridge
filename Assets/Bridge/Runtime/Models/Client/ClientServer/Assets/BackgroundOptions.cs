using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public sealed class BackgroundOptions
    {
        public SetLocationBackground[] Backgrounds { get; set; } = Array.Empty<SetLocationBackground>();

        public SetLocationBackgroundSettings[] BackgroundSettings { get; set; } = Array.Empty<SetLocationBackgroundSettings>();
        
        public IEnumerable<IBackgroundOption> Options => Backgrounds.Concat(BackgroundSettings.Cast<IBackgroundOption>()).OrderBy(x => x.SortOrder);
    }

    public enum BackgroundOptionType
    {
        Image,
        GenerationSettings
    }

    public interface IBackgroundOption: IThumbnailOwner
    { 
        string Name { get; set; }
        int? SortOrder { get; set; }
        BackgroundOptionType Type { get; }
    }
    
    public class SetLocationBackgroundSettings: IBackgroundOption
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }
        public BackgroundSettings Settings { get; set; }
        public BackgroundOptionType Type => BackgroundOptionType.GenerationSettings;
        public List<FileInfo> Files { get; set; }
    }
    
    
    public class BackgroundSettings
    {
        public string ModelVersion { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int DiffusionSteps { get; set; }
        public float LoraScale { get; set; }
        public float GuidanceScale { get; set; }
        public BackgroundPrompt[] Prompts { get; set; }
        public BackgroundOptionsSet[] Sets { get; set; }
    }
    
    
    public class BackgroundPrompt
    {
        public int Weight { get; set; }
        public string Text { get; set; }
    }

    public class BackgroundOptionsSet
    {
        public string Title { get; set; }
        public int ColumnsCount { get; set; }
        public BackgroundOption[] Options { get; set; }
    }

    public class BackgroundOption
    {
        public string DisplayValue { get; set; }
        public string PromptValue { get; set; }
        public string Label { get; set; }
    } 
}