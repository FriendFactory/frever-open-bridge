using System;
using Bridge.AccountVerification.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.AccountVerification
{
    public sealed class ChangePasswordTest: VerificationMethodTestBase
    {
        [SerializeField] private string _oldPassword = "password3000!";
        [SerializeField] private string _newPassword = "newPassword3000!";
        
        protected override async void RunTestAsync()
        {
            try
            {
                var verifyUserResult = await Bridge.VerifyCredentials(CredentialType.Password, _oldPassword);
                if (verifyUserResult.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to verify user # code: {verifyUserResult.HttpStatusCode}, msg: {verifyUserResult.ErrorMessage}");
                    return;
                }

                var verifyUserResponse = verifyUserResult.Model;
                var verificationToken = verifyUserResponse.VerificationToken;

                var result = await Bridge.ChangeVerificationMethod(_newPassword, verificationToken);
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to remove verification method # code: {result.HttpStatusCode}, msg: {result.ErrorMessage}");
                    return;
                }

                var credentialsStatus = await GetCredentialStatusAsync();
                
                Assert.IsTrue(credentialsStatus.HasPassword);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}