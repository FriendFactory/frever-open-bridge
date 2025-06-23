using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    internal sealed class GetStarCreatorsProfileTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetStarCreatorsInYourCountry();
            if (resp.IsError)
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(resp.Profiles.Length)}");
            }
        }
    }
}