using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using NUnit.Framework;

namespace Tests.MainServer.PrimitiveFieldsRequestTests
{
    public class PrimitiveRequestsTest 
    {
        [Test]
        public void TryCreateRequestWithNullableLong()
        {
            var user = new User();
            user.MainCharacterId = 10;
            
            var request = new PrimitiveFieldsUpdateReq<User>(user);
            request.UpdateProperty(nameof(user.MainCharacterId));
            
            Assert.IsTrue(request.HasDataToUpdate);
        }
    }
}
