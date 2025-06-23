using System;
using System.Threading.Tasks;
using Bridge.AccountVerification.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AccountVerification
{
    public class VerifyEmailOrPasswordTest : AuthorizedUserApiTestBase 
    {
        [SerializeField] private VerifiableCredentialType _type;
        [SerializeField] private string _credential; 
        [SerializeField] private string _verificationCode; 
        
        protected override async void RunTestAsync()
        {
            try
            {
                var type = _type == VerifiableCredentialType.Email ? CredentialType.Email : CredentialType.PhoneNumber;
                
                var verificationCodeResult = await Bridge.SendVerificationCode(_type, _credential, false);
                if (verificationCodeResult.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to send verification code # code: {verificationCodeResult.HttpStatusCode}, msg: {verificationCodeResult.ErrorMessage}");
                    return;
                }
    
                while (string.IsNullOrEmpty(_verificationCode) || _verificationCode.Length != 6)
                {
                    await Task.Delay(42);
                }
    
                var verifyUserResult = await Bridge.VerifyCredentials(type, _verificationCode);
                if (verifyUserResult.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to verify user # code: {verifyUserResult.HttpStatusCode}, msg: {verifyUserResult.ErrorMessage}");
                    return;
                }
    
                var verifyUserResponse = verifyUserResult.Model;
                
                Assert.IsTrue(verifyUserResponse.IsSuccessful);
                Assert.IsTrue(!string.IsNullOrEmpty(verifyUserResponse.VerificationToken));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
