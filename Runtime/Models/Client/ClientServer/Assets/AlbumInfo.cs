using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Assets
{
    public class AlbumInfo: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}