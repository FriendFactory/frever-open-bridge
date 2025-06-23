using System.Threading.Tasks;
using Bridge.Services.UserProfile;
using UnityEngine;

namespace ApiTests.AdTrackingTests
{
    public class AddTrackingTest : AuthorizedUserApiTestBase
    {
        /*
         * AppsFlyer doesn't work in editor so I had to hard code ID from one of our test devices might stop working in
         * the future.
         */
        private const string TEST_ID = "1699361711244-7826171911696288464";
        
        protected override async void RunTestAsync()
        {
            if (await IsTrackingEnabled()) RemoveTrackingIds();
            
            if (await IsTrackingEnabled())
            {
                Debug.LogError("Ids removed unsuccessfully");
                return;
            }

            Debug.Log("Testing AddTracking Id");
            var additionResult = await Bridge.AddTrackingId(TEST_ID);
            if (additionResult.IsError)
            {
                Debug.LogError(additionResult.ErrorMessage);
                return;
            }

            if (!await IsTrackingEnabled())
            {
                Debug.LogError("Add Tracking Id failed");
                return;
            }
            
            RemoveTrackingIds();
            if (await IsTrackingEnabled())
            {
                Debug.LogError("Ids removed unsuccessfully");
            }
            
            Debug.Log("PASSED");
        }

        private async Task<bool> IsTrackingEnabled()
        {
            var profile = await GetUserInfo();
            var trackingEnabled = profile.AdvertisingTrackingEnabled;
            return trackingEnabled;
        }
        
        private async Task<MyProfile> GetUserInfo()
        {
            var userInfoResult = await Bridge.GetCurrentUserInfo();
            if (userInfoResult.IsError) Debug.Log(userInfoResult.ErrorMessage);

            return userInfoResult.Profile;
        }
        
        private async void RemoveTrackingIds()
        {
            var removalResult = await Bridge.RemoveAllTrackingIds();
            if (removalResult.IsError) Debug.Log(removalResult.ErrorMessage);
        }
    }
}