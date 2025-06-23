using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Services.UserProfile;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    public class BlockUserProfileTest : AuthorizedUserApiTestBase
    {
        public long GroupIdToBlock;
        
        protected override async void RunTestAsync()
        {
            var blockingResponse = await Bridge.BlockUser(GroupIdToBlock);
            if (blockingResponse.IsSuccess)
            {
                Debug.Log($"Account {GroupIdToBlock} is blocked");
            }
            else
            {
                Debug.LogError($"Failed to block account. Reason {blockingResponse.ErrorMessage}");
            }

            await VerifyAccountIsBlocked(GroupIdToBlock);
        }

        private async Task VerifyAccountIsBlocked(long groupId)
        {
            var blockedAccounts = await GetBlockedProfiles();
            if (blockedAccounts.All(x => x.MainGroupId != groupId))
            {
                throw new Exception("Blocked account is not presented in the list");
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
