using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.InvitationCodeTests
{
    public class GetSuggestedNamesTest: AnonymousAccessedEndPointTest
    {
        [SerializeField] private int _count = 5;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetSuggestedNicknames(_count);
            Debug.Log($"Response: {JsonConvert.SerializeObject(resp)}");
        }
    }
}