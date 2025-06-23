using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ChatTests
{
    internal sealed class GetChatMessage : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _chatId;

        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetChatMessages(_chatId, null, 10, 10);
                
            Debug.Log($"Result: {JsonConvert.SerializeObject(resp)}");
        }
    }
}