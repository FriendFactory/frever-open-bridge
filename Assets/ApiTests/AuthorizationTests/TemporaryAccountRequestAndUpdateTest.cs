using System;
using System.Threading.Tasks;
using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class TemporaryAccountRequestAndUpdateTest : MonoBehaviour
    {
        [SerializeField] private FFEnvironment _environment;
        [SerializeField] private string _email;
        [SerializeField] private string _username;
        [SerializeField] private string _verificationCode;
        
        private IBridge _bridge;

        private async void Start()
        {
            _bridge = new ServerBridge();
            _bridge.ChangeEnvironment(_environment);

            var model = new TemporaryAccountRequestModel
            {
                Country = "swe",
                DefaultLanguage = "swe",
            };
            var result = await _bridge.RegisterTemporaryAccount(model);
            if (result.IsError)
            {
                Debug.LogError(result.ErrorMessage);
                return;
            }
            
            Debug.Log("Temporary account request: Successful");

            await _bridge.RequestEmailVerificationCode(_email);
            Debug.Log($"Verification code requested: {_email}");
            
            while (string.IsNullOrEmpty(_verificationCode) || _verificationCode.Length!=6)
            {
                await Task.Delay(100);
            }
        }
    }
}