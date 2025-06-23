using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Services.UserProfile;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    public class UnblockUserProfileTest: AuthorizedUserApiTestBase
    {
        public long GroupIdToUnblock;
        
        protected override async void RunTestAsync()
        {
            var blockingResponse = await Bridge.UnBlockUser(GroupIdToUnblock);
            if (blockingResponse.IsSuccess)
            {
                Debug.Log($"Account {GroupIdToUnblock} is unblocked");
            }
            else
            {
                Debug.LogError($"Failed to unblock account. Reason {blockingResponse.ErrorMessage}");
            }

            await VerifyAccountIsUnBlocked(GroupIdToUnblock);
        }

        private async Task VerifyAccountIsUnBlocked(long groupId)
        {
            var blockedAccounts = await GetBlockedProfiles();
            if (blockedAccounts.Any(x => x.MainGroupId == groupId))
            {
                throw new Exception("Unblocked account is still in the list");
            }
            
            Debug.Log("Account blocking is confirmed");
        }

        private async Task<Profile[]> GetBlockedProfiles()
        {
            var blockedProfilesResult = await Bridge.GetBlockedProfiles();
            if (blockedProfilesResult.IsError)
            {
                throw new Exception($"Failed to download blocked profiles. Reason: {blockedProfilesResult.ErrorMessage}");
            }
            return blockedProfilesResult.Profiles;
        } 
    }
}