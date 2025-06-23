using Bridge;
using Bridge.Authorization;
using Bridge.Authorization.LocalStorage;
using Bridge.Authorization.LocalStorage.Storage;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.MainServer.SavingUserCredsLocally
{
    public class LocalUserInfoSavingTests
    {
        [Test]
        public void SaveEnvironmentValue_AfterReloadingMustBeTheSame()
        {
            var userInfo = new UserData
            {
                FfEnvironment = FFEnvironment.Test, 
                Token = new AuthToken("testToken", "refreshToken")
            };

            var saver = new UserDataStorage();
            saver.Save(userInfo);
            saver.Load();

            Assert.IsTrue(saver.HasSavedData);
            Assert.AreEqual(saver.UserData.FfEnvironment, saver.UserData.FfEnvironment);
            Assert.AreEqual(saver.UserData.Token, saver.UserData.Token);
        }
    }
}
