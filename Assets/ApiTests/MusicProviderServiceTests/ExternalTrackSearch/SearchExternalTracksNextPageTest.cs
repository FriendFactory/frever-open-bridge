using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.MusicProviderServiceTests.ExternalTrackSearch
{
    public class SearchExternalTracksNextPageTest: SearchExternalTrackTestBase 
    {
        [SerializeField] private string _searchQuery = "wham";
        [SerializeField] private int _take = 5;

        protected override async Task TestSearchAsync()
        {
            var firstPage = await Bridge.SearchExternalTracks(_searchQuery, _take);
            
            if (firstPage.IsError)
            {
                Debug.LogError($"[{GetType().Name}] External tracks search failed # {firstPage.ErrorMessage}");
                return;
            }

            var firstPageModels = firstPage.Models;

            Assert.IsTrue(firstPageModels.Length == 5, "First page size equals take parameter");

            var nextPage = await Bridge.SearchExternalTracks(_searchQuery, _take, _take);
            
            if (nextPage.IsError)
            {
                Debug.LogError($"[{GetType().Name}] External tracks search failed # {firstPage.ErrorMessage}");
                return;
            }

            var nextPageModels = nextPage.Models;
            
            Assert.IsTrue(nextPageModels.Length == 5, "Next page equals take parameter");

            var firstPageIds = new HashSet<long>(firstPageModels.Select(model => model.ExternalTrackId));
            var nextPageIds = new HashSet<long>(nextPageModels.Select(model => model.ExternalTrackId));

            Assert.IsTrue(!firstPageIds.Overlaps(nextPageIds), "First and next pages do not overlap");
        }
    }
}