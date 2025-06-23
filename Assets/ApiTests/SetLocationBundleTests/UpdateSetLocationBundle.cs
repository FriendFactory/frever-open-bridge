using System;
using Bridge.Models.AsseManager;

namespace ApiTests.SetLocationBundleTests
{
    public class UpdateSetLocationBundle : EntityApiTest<SetLocationBundle>
    {
        protected override async void RunTestAsync()
        {
            var anyBundleId = await GetAnyAvailableEntityId<SetLocationBundle>();
            var setLocationBundle = (await Bridge.GetAsync<SetLocationBundle>(anyBundleId)).ResultObject;
            setLocationBundle.Name = DateTime.Now.ToString();

            var updateResult = await Bridge.UpdateAsync(setLocationBundle);
            LogResult(updateResult);
        }
    }
}
