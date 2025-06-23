using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    internal sealed class GetPublicProfileTest: AuthorizedUserApiTestBase
    {
        public string NickName;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetPublicProfileFor(NickName);
            if (resp.IsError)
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(resp.Profile)}");
            }
        }
    }
}