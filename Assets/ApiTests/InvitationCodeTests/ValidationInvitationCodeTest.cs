using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.InvitationCodeTests
{
    public class ValidationInvitationCodeTest : AnonymousAccessedEndPointTest
    {
        public string InvitationCode;
    
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.ValidateInvitationCode(InvitationCode);
            Debug.Log($"Response: {JsonConvert.SerializeObject(resp)}");
        }
    }
}
