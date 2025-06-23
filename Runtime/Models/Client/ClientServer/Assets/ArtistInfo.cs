using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Assets
{
    public class ArtistInfo: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}