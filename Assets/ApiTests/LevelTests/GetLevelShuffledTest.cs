using System.Collections.Generic;
using System.Linq;
using Bridge.Models.ClientServer.Level.Shuffle;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.LevelTests
{
    internal sealed class GetLevelShuffledTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var setLocationResp = await Bridge.GetSetLocationListAsync(null, 10, 0, 1);
            var setLocation = setLocationResp.Models.First();
            var spawnPos = setLocation.CharacterSpawnPosition.First(x => x.AvailableForSelection);

            var characterResp = await Bridge.GetMyCharacters(null, 10, 0, 1);
            var character = characterResp.Models.First();

            var bodyAnimResp = await Bridge.GetBodyAnimationListAsync(null, 10, 10, 1);
            var bodyAnim = bodyAnimResp.Models.First();
            
            var shuffleInput = new LevelShuffleInput();
            shuffleInput.CharacterCount = 1;
            shuffleInput.ShuffleAssets = ShuffleAssets.BodyAnimation;

            var events = new List<InputEventInfo>();
            events.Add(new InputEventInfo
            {
                SetLocationId = setLocation.Id,
                Characters = new []{ new CharacterInputInfo()
                {
                    CharacterId = character.Id,
                    CharacterSpawnPositionId = spawnPos.Id,
                    BodyAnimationId = bodyAnim.Id
                }}
            });

            shuffleInput.Events = events;

            var resp = await Bridge.GetShuffledLevel(shuffleInput);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}