using System;
using Bridge.AccountVerification.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AccountVerification
{
    public sealed class VerifyPasswordTest: VerificationMethodTestBase
    {
        [SerializeField] private string _password = "password3000!";
        
        protected override async void RunTestAsync()
        {
            try
            {
                var verifyUserResult = await Bridge.VerifyCredentials(CredentialType.Password, _password);
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