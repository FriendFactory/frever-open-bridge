using System;
using Bridge.Models.AsseManager;

namespace ApiTests.Tags
{
    public class CreateTag : EntityApiTest<Tag>
    {
        protected override async void RunTestAsync()
        {
            var tag = new Tag();
            tag.Name = "SerhiiTest_" + DateTime.Now;
            tag.CategoryId = await GetAnyAvailableEntityId<TagCategory>();
            var resp = await Bridge.PostAsync(tag);
            LogResult(resp);
        }
    }
}
