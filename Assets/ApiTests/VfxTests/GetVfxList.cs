using System;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VfxTests
{
    public class GetVfxList: EntityApiTest<Vfx>
    {
        [SerializeField] private bool _withAnimationOnly;
        
        protected override async void RunTestAsync()
        {
            try
            {
                var vfxResp = await Bridge.GetVfxListAsync(null, 10, 0, 1, withAnimationOnly: _withAnimationOnly);
                Debug.Log(JsonConvert.SerializeObject(vfxResp.Models.Length));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}