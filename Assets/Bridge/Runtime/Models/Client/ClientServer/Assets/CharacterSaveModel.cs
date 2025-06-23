using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public sealed class CharacterSaveModel: IThumbnailOwner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long GenderId { get; set; }
        public long CharacterStyleId { get; set; }
        public long GroupId { get; set; }
        public List<FileInfo> Files { get; set; }
        public UmaRecipeFullInfo UmaRecipe { get; set; }
        public long[] WardrobeIds { get; set; }
        public long UniverseId { get; set; }
    }
}