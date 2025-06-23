using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.GroupTests
{
    internal sealed class GetGroupsTest : EntityApiTest<Group>
    {
        protected override async void RunTestAsync()
        {
            var q = new Query<Group>();
            q.SetFilters(new FilterSetup()
            {
                FieldName = nameof(Group.Id),
                FilterType = FilterType.Equals,
                FilterValue = 16
            });
            var resp = await Bridge.GetAsync(q);
            Debug.Log(JsonConvert.SerializeObject(resp.Models));
        }
    }
}




