using System;
using System.Threading;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.WardrobeTests
{
    public class DownloadWardrobeIcon: EntityApiTest<Wardrobe>
    {
        private CancellationTokenSource _cancellation;
        
        protected override async void RunTestAsync()
        {
            _cancellation = new CancellationTokenSource();
            
            var id = await GetAnyAvailableEntityId<Wardrobe>();
            var wardrobeResp = await Bridge.GetAsync<Wardrobe>(id);

            var resp = await Bridge.GetThumbnailAsync(wardrobeResp.ResultObject, Resolution._128x128, true, _cancellation.Token);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cancellation.Cancel();
            }
        }
    }
}