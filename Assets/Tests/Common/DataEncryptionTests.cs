using System;
using Bridge;
using NUnit.Framework;

namespace Tests.Common
{
    public class DataEncryptionTests
    {
        private readonly DataEncryptionHelper _dataEncryptionHelper = new DataEncryptionHelper();
        
        [Test]
        public async void EncryptDataTest()
        {
            var dataToEncrypt = "Token: 1238712312ahwduyawgidawuih1234da, Environment: Production";
            var encryptedData = await _dataEncryptionHelper.Encrypt(dataToEncrypt);

            Assert.AreNotEqual(dataToEncrypt, encryptedData);
        }
        
        [Test]
        public async void DecryptDataTest()
        {
            var dataToEncrypt = "Token: 1238712312ahwduyawgidawuih1234da, Environment: Production";
            var encryptedData = await _dataEncryptionHelper.Encrypt(dataToEncrypt);
            var decryptedData = _dataEncryptionHelper.Decrypt(encryptedData);

            Assert.AreEqual(dataToEncrypt, decryptedData);
        }
    }
}