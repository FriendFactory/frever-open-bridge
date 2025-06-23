using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.MusicProviderServiceTests.ExternalTrackSearch
{
    public class SearchExternalTracksTest: SearchExternalTrackTestBase 
    {
        [SerializeField] private string _searchQuery = "wham";

        protected override async Task TestSearchAsync()
        {
            var result = await Bridge.SearchExternalTracks(_searchQuery);
            
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] External tracks search failed # {result.ErrorMessage}");
                return;
            }

            Assert.IsTrue(result.Models.Length > 0);
        }
    }
}