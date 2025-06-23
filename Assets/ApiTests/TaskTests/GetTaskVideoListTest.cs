using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetTaskVideoListTest: AuthorizedUserApiTestBase
    {
        public long TaskId;
        public string VideoKey;
        public int TakeNext = 10;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetTaskVideoListAsync(TaskId, VideoKey, TakeNext);
            if (resp.IsError)
            {
                Debug.LogError(resp.ErrorMessage);
            }
            else
            {
                Debug.Log(resp.Models.Length);
            }
        }
    }
}