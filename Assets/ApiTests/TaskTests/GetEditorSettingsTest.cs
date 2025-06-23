using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetEditorSettingsTest: AuthorizedUserApiTestBase
    {
        public long TaskId;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetSettingForTask(TaskId);
            if (resp.IsError)
            {
                Debug.LogError($"Error: {resp.ErrorMessage}");
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(resp.Model)}");
            }
        }
    }
}