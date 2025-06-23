using System.IO;
using Bridge;
using Bridge.Authorization;
using Bridge.Authorization.LocalStorage;
using Newtonsoft.Json;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests.Performance
{
    public class ReadUserDataTest
    {
        private readonly JsonUserDataHandler _jsonUserDataHandler = new JsonUserDataHandler();
        private readonly EncryptedUserDataHandler _encryptedUserDataHandler = new EncryptedUserDataHandler();
        
        [Test, Performance]
        public void EncryptedReadDataTest()
        {
            if(!_encryptedUserDataHandler.HasSavedFile) _encryptedUserDataHandler.SaveFile(GetUserData());
            
            Measure.Method(() => { _encryptedUserDataHandler.ReadFile(); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }
        
        [Test, Performance]
        public void JsonReadDataTest()
        {
            if(!_jsonUserDataHandler.HasSavedFile) WriteDataFile(GetUserData());
            
            Measure.Method(() => { _jsonUserDataHandler.ReadFile(); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }
        
        private void WriteDataFile(UserData data)
        {
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(Application.persistentDataPath + "/auth_data", json);
        }

        private UserData GetUserData()
        {
            return new UserData {FfEnvironment = FFEnvironment.Production, Token = new AuthToken("Test", "adwdygawuifg6218hawdhaadwauhi21389awdwaoddh128731y9eyuge18g2eg872eg91")};
        }
    }
}