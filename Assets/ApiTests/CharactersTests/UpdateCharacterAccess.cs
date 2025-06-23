using Bridge.Models.ClientServer.Assets;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CharactersTests
{
    internal sealed class UpdateCharacterAccess: AuthorizedUserApiTestBase
    {
        public CharacterAccess Access;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.UpdateCharacterAccess(Access);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}