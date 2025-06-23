using System;
using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CharacterFullInfo: IThumbnailOwner, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long GenderId { get; set; }
        public long GroupId { get; set; }
        public long CharacterStyleId { get; set; }
        public bool IsFreverStar { get; set; }
        public CharacterAccess Access { get; set; }
        public List<FileInfo> Files { get; set; }
        public UmaRecipeFullInfo UmaRecipe { get; set; }
        public List<WardrobeFullInfo> Wardrobes { get; set; }
        [ProtoNewField(1)] public BakedView[] BakedViewsObsolete { get; set; }
        [ProtoNewField(2)] public Guid Version { get; set; }
        [ProtoNewField(3)] public BakedView[] BakedViewsObsolete2 { get; set; }
        [ProtoNewField(4)] public BakedView[] BakedViewsObsolete3 { get; set; }
        [ProtoNewField(5)] public BakedView[] BakedViewsObsolete4 { get; set; }
        [ProtoNewField(6)] public BakedView[] BakedViews { get; set; }
    }
}