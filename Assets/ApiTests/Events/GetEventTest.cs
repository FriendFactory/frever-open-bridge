using Newtonsoft.Json;
using UnityEngine;
using Event = Bridge.Models.AsseManager.Event;

namespace ApiTests.Events
{
    public sealed class GetEventTest: EntityApiTest<Event>
    {
        protected override async void RunTestAsync()
        {
            var id = await GetAnyAvailableEntityId<Event>();
            var resp = await Bridge.GetAsync<Event>(id);
            
            Debug.Log(JsonConvert.SerializeObject(resp.ResultObject));
        }
    }
}