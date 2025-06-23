using System.IO;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;

namespace ApiTests.SetLocationTests
{
    public class ComparingSetLocationUpdate: EntityApiTest<SetLocation>
    {
        public string OriginalJsonFileName = "originalJson.json";
        public string ModifiedJsonFileName = "modifiedJson.json";

        protected override async void RunTestAsync()
        {
            var originJson = File.ReadAllText(GetFilePath(OriginalJsonFileName));
            var modifiedJson = File.ReadAllText(GetFilePath(ModifiedJsonFileName));

            var originModel = JsonConvert.DeserializeObject<SetLocation>(originJson);
            var modifiedModel = JsonConvert.DeserializeObject<SetLocation>(modifiedJson);

            var query = new DifferenceDeepUpdateReq<SetLocation>(originModel, modifiedModel);

            var res = await Bridge.UpdateAsync(query);
            LogResult(res);
        }
    }
}