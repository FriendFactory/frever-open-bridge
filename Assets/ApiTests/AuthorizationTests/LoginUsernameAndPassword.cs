using Bridge;
using Bridge.Authorization.Models;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class LoginUsernameAndPassword : MonoBehaviour
    {
        [SerializeField] private FFEnvironment _environment;
        [SerializeField] private string _username;
        [SerializeField] private string _password;

        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(_environment);

            var creds = new UsernameAndPasswordCredentials
            {
                Email = string.Empty,
                Username = _username,
                Password = _password,
            };
            var result = await bridge.LogInAsync(creds, false);
            if (result.IsError)
            {
                Debug.LogError(result.ErrorMessage);
                return;
            }
            
            Debug.Log("Login completed");
        }
    }
}