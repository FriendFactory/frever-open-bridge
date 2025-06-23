using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ApiTests.MusicProviderServiceTests.ExternalTrackSearch
{
    public abstract class SearchExternalTrackTestBase: AuthorizedUserApiTestBase
    {
        [SerializeField]
        [Tooltip("Endpoint performs additional country based filtering, so, test could be run correctly only if you are in country that is added to whitelist")]
        private bool _enabledInMyCountry;
        
        protected override async void RunTestAsync()
        {
            try
            {
                var result = await Bridge.GetCurrentUserInfo();
                var musicEnabled = result.Profile.MusicEnabled && _enabledInMyCountry;

                if (!musicEnabled)
                {
                    Debug.LogWarning($"[{GetType().Name}] External music provider is disabled for this user - test skipped");
                    return;
                }
                
                await TestSearchAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected abstract Task TestSearchAsync();
    }
}