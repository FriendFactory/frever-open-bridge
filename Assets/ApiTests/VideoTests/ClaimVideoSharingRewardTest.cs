using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace ApiTests.VideoTests
{
    public sealed class ClaimVideoSharingRewardTest: AuthorizedUserApiTestBase
    {
        private const string VIDEO_LINK_REGEX = @"^https://web\.frever-api\.com/video/([a-z])_([a-zA-Z0-9]+)$";
        
        [SerializeField] private long _videoId;
        
        protected override async void RunTestAsync()
        {
            var sharingInfoResult = await Bridge.GetVideoSharingInfo(_videoId);
            if (sharingInfoResult.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get video sharing info # {sharingInfoResult.ErrorMessage}");
                return;
            }

            var sharingInfo = sharingInfoResult.Model;
            var shareCount = sharingInfo.CurrentShareCount;
            
            if (shareCount == sharingInfo.RewardedShareCount)
            {
                Debug.LogWarning($"The daily video sharing limit is reached");
            }

            if (!TryGetVideoGuid(sharingInfo.SharedPlayerUrl, out var videoGuid))
            {
                Debug.LogError($"[{GetType().Name}] Failed to get video guid");
                return;
            }

            var result = await Bridge.ClaimVideoShareReward(videoGuid);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to claim video share reward # {result.ErrorMessage}");
                return;
            }
            
            sharingInfoResult = await Bridge.GetVideoSharingInfo(_videoId);
            if (sharingInfoResult.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get video sharing info # {sharingInfoResult.ErrorMessage}");
                return;
            }
            
            Assert.AreEqual(shareCount + 1, sharingInfoResult.Model.CurrentShareCount);
        }

        private bool TryGetVideoGuid(string link, out string videoGuid)
        {
            videoGuid = string.Empty;
            
            var videoRegex = new Regex(VIDEO_LINK_REGEX);
            var match = videoRegex.Match(link);

            if (!match.Success) return false;

            videoGuid = match.Groups[2].Value;

            return true;
        }
    }
}