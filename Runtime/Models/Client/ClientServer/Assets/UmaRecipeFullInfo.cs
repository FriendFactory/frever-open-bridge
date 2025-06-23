using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Assets
{
    public class UmaRecipeFullInfo: IEntity
    {
        public long Id { get; set; }
        public byte[] J { get; set; }
    }
}