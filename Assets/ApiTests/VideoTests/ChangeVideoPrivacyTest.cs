using Bridge.Models.VideoServer;
using Bridge.VideoServer;
using UnityEngine;

namespace ApiTests.VideoTests
{
    public class ChangeVideoPrivacyTest : EntityApiTest<Video>
    {
        public VideoAccess AccessLevel;
        public long Id = 1;

        protected override async void RunTestAsync()
        {
            var resp = await Bridge.ChangePrivacyAsync(Id, new UpdateVideoAccessRequest()
            {
                Access = AccessLevel
            });
            if (resp.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError("Error: " + resp.ErrorMessage);
            }
        }
    }
}