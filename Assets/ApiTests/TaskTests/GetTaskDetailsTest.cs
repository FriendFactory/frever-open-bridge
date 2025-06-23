using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetTaskDetailsTest: AuthorizedUserApiTestBase
    {
        public long Id;
        
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetTaskFullInfoAsync(Id);
            if (res.IsError)
            {
                Debug.LogError(res.ErrorMessage);
            }
            else
            {
                Debug.Log(JsonConvert.SerializeObject(res.Model));
            }
        }
    }
}