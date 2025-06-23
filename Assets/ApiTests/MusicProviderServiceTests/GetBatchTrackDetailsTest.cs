using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.MusicProviderServiceTests
{
    public class GetBatchTrackDetailsTest : AuthorizedUserApiTestBase
    {
        [SerializeField][Range(5, 60)] private int _batchSize = 10;
    
        private readonly long[] _trackIds = new long[]
        {
            3103566, 3104313, 8282220, 16267164, 3104310, 16267174, 3259068, 3259088, 66840314, 3104307,
            3104316, 6317279, 6317553, 16267145, 3258639, 3103567, 3103568, 6317281, 16267158, 39037628,
            4696636, 3104314, 3104308, 22287490, 16267177, 3259076, 3259082, 3104309, 16267152, 4696693,
            16267155, 16267183, 3104317, 3527057, 4696691, 16267196, 3127982, 4696692, 6317280, 6317301,
            21888719, 17898615, 3104312, 6317335, 7038981, 6935084, 58624526, 3104311, 21854578, 13910768,
            15725258, 8929801, 8707745, 9018855, 13915216, 39060467, 141293327, 157992779, 6337996, 8735458,
        };
    
        protected override async void RunTestAsync()
        {
            var ids = _trackIds.Take(_batchSize).ToArray();
        
            var result = await Bridge.GetBatchTrackDetails(ids);
        
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get tracks details # {result.ErrorMessage}");
                return;
            }

            var models = result.Models;

            var initialIds = new HashSet<long>(ids);
            var resultIds = new HashSet<long>(models.Select(model => model.Id));
        
            Assert.IsTrue(initialIds.SetEquals(resultIds));
        }
    }
}
