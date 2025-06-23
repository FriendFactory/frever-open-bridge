using System;
using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class RegistrationUsernameAndPassword : MonoBehaviour
    {
        [SerializeField] private FFEnvironment _environment;
        [SerializeField] private string _username;
        [SerializeField] private string _password;

        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(_environment);

            var model = new UserRegistrationModel
            {
                BirthDate = DateTime.Now,
                Country = "SWE",
                Credentials = new UsernameAndPasswordCredentials
                {
                    Password = _password,
                    Username = _username,
                },
                DefaultLanguage = "swe",
                UserName = _username,
            };
            var result = await bridge.RegisterAsync(model, false);
            if (result.IsError)
            {
                Debug.LogError($"Registration failed: {result.ErrorMessage}");
            }
            
            Debug.Log("Registration completed");
        }
    }
}