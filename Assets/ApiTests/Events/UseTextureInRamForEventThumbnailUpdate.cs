using Bridge.AssetManagerServer;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using UnityEngine;
using Event = Bridge.Models.AsseManager.Event;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.Events
{
    public class UseTextureInRamForEventThumbnailUpdate : EntityApiTest<Event>
    {
        protected override async void RunTestAsync()
        {
            var evId = await GetAnyAvailableEntityId<Event>();
            var getEventResponse = await Bridge.GetAsync<Event>(evId);

            var ev = getEventResponse.ResultObject;

            var clone = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(ev));
            
            clone.Files.Clear();
            var texture1 = new Texture2D(200,200);
            var texture2 = new Texture2D(300,300);
            
            var fileInfo1 = new FileInfo(texture1, FileExtension.Png, Resolution._128x128);
            var fileInfo2 = new FileInfo(texture2, FileExtension.Png, Resolution._512x512);
            
            clone.Files.Add(fileInfo1);
            clone.Files.Add(fileInfo2);
            
            var deepDiffRequest = new DifferenceDeepUpdateReq<Event>(ev, clone);
            var resp = await Bridge.UpdateAsync(deepDiffRequest);
            if(resp.IsError)
                Debug.LogError(resp.ErrorMessage);
            else 
                Debug.Log("Id : " + ev.Id);
        }
    }
}
