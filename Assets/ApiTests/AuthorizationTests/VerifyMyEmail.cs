using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    internal sealed class VerifyMyEmail : AuthorizedUserApiTestBase
    {
        [SerializeField] private string _code;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.RequestMyParentEmailVerificationCode();
            Debug.Log(JsonConvert.SerializeObject(resp));
            while (string.IsNullOrEmpty(_code) || _code.Length != 6)
            {
                await Task.Delay(100);
            }

            resp = await Bridge.VerifyMyParentEmail(_code);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}
