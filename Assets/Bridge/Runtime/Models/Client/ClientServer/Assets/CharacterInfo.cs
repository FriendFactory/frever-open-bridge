using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class CharacterInfo: IThumbnailOwner, INamed, INewTrackable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<FileInfo> Files { get; set; }
        public CharacterAccess Access { get; set; }
        public long GenderId { get; set; }
        public long GroupId { get; set; }
        public bool IsNew { get; set; }
        public bool IsFreverStar { get; set; }
    }
    
    public enum CharacterAccess
    {
        Private = 0,
        ForFriends = 1,
    }
}