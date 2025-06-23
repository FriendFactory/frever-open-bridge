using System.Linq;
using ApiTests;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

public class DeleteVfxGroup : EntityApiTest<SetLocation>
{
    protected override async void RunTestAsync()
    {
        var q = new Query<VfxPositionGroup>();
        q.SetMaxTop(30);
        var vfxGroupsRes = await Bridge.GetAsync(q);
        var vfxGroup = vfxGroupsRes.Models.GroupBy(x => x.SetLocationId).First(x => x.Count() > 1).First();

        var query = new Query<SetLocation>();
        query.ExpandField(nameof(SetLocation.VfxPositionGroup));
        query.SetFilters(new FilterSetup(nameof(SetLocation.Id), FilterType.Equals, vfxGroup.SetLocationId));
        var setLocation = (await Bridge.GetAsync(query)).Models.First();
     
        var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(setLocation));
        modified.VfxPositionGroup.Remove(modified.VfxPositionGroup.First(x => x.Id == vfxGroup.Id));
        Debug.Log("Deleted vfx position group: " + vfxGroup.Id);

        var diffQuery = new DifferenceDeepUpdateReq<SetLocation>(setLocation, modified);

        var resp = await Bridge.UpdateAsync(diffQuery);
        LogResult(resp);

    }
}
