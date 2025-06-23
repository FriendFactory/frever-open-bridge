using System;
using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AccountVerification
{
    public sealed class AddEmailOrPasswordTest: VerificationMethodTestBase 
    {
        [SerializeField] private VerifiableCredentialType _type;
        [SerializeField] private string _credential; 
        [SerializeField] private string _verificationCode; 
        
        protected override async void RunTestAsync()
        {
            try
            {
                var verificationCodeResult = await Bridge.SendVerificationCode(_type, _credential, true);
                if (verificationCodeResult.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to send verification code # code: {verificationCodeResult.HttpStatusCode}, msg: {verificationCodeResult.ErrorMessage}");
                    return;
                }

                while (string.IsNullOrEmpty(_verificationCode) || _verificationCode.Length != 6)
                {
                    await Task.Delay(42);
                }

                var result = await Bridge.AddVerificationMethod(_type, _credential, _verificationCode);
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to verify phone number # code: {result.HttpStatusCode}, msg: {result.ErrorMessage}");
                    return;
                }

                var credentialStatus = await GetCredentialStatusAsync();
                var credential = _type == VerifiableCredentialType.Email ? credentialStatus.Email : credentialStatus.PhoneNumber;

                Assert.IsTrue(_credential == credential);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}