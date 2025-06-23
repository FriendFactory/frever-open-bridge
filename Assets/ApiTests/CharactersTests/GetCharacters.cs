using System.Linq;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CharactersTests
{
    internal sealed class GetCharacters : EntityApiTest<Character>
    {
        protected override async void RunTestAsync()
        {
            var characters = await Bridge.GetStarCharacters(null,10, 0, 1);
            Debug.Log( JsonConvert.SerializeObject(characters.Models.Select(x=>new
            {
                x.Id,
                x.Name
            })));
        }
    }
}
