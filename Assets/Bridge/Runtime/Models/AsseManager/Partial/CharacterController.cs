using Bridge.Models.Common;

namespace Bridge.Models.AsseManager
{
    public partial class CharacterController : IEntity, IOrderable
    {
        public int OrderNumber => ControllerSequenceNumber;
    }
}
