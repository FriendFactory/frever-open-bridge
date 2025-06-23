using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CharactersTests
{
    internal sealed class GetFriendsCharacters: AuthorizedUserApiTestBase
    {
        public int TakeNext = 10;
        public int TakePrevious = 0;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetFriendsMainCharacters(null, TakeNext, TakePrevious, 1);
            if (resp.IsError)
            {
                Debug.LogError($"{resp.ErrorMessage}");
                return;
            }
            
            Debug.Log(JsonConvert.SerializeObject(resp.Models));
        }
    }
}