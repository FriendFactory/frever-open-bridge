using System;
using System.Collections.Generic;
using Bridge;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ApiTests.MusicTests
{
    public class GetSongTest: AuthorizedUserApiTestBase
    {
        // TODO: move to dedicated SO that will hold music related test data for each environment
        private static readonly Dictionary<FFEnvironment, long[]> SONG_IDS_MAP = new Dictionary<FFEnvironment, long[]>()
        {
            { FFEnvironment.Develop,  new long[] {117, 116, 115, 114, 113}},
        };

        protected override async void RunTestAsync()
        {
            try
            {
                if (!SONG_IDS_MAP.TryGetValue(Environment, out var ids))
                {
                    Debug.LogError($"[{GetType().Name}] There is no known song ids for {Environment} to run test");
                    return;
                }

                var id = ids[Random.Range(0, ids.Length)];
                var result = await Bridge.GetSongAsync(id, default);
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to get song with id {id}: {result.ErrorMessage} ");
                    return;
                }

                Debug.Log($"{JsonConvert.SerializeObject(result.Model)}");
                
                Assert.IsTrue(result.Model.Id == id);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
    }
}