using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.BattleTests
{
    internal sealed class GetBattlesForTask : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _taskId;
        
        protected override async void RunTestAsync()
        {
            var battlesResp = await Bridge.GetVotingBattlePairs(_taskId);
            if (battlesResp.IsError)
            {
                Debug.LogError($"Error: {battlesResp.ErrorMessage}");
            }
            else
            {
                Debug.Log(JsonConvert.SerializeObject(battlesResp.Models));
            }
        }
    }
}
