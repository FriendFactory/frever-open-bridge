using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.VideoServer;
using Bridge.VideoServer;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;

namespace ApiTests.VideoTests
{
    public sealed class UploadLevelVideoTest: EntityApiTest<Video>
    {
        protected override async void RunTestAsync()
        {
            var query = new Query<Level>();
            query.SetFilters(new FilterSetup(nameof(Level.GroupId), FilterType.Equals, Bridge.Profile.GroupId));
            query.SetOrderBy(nameof(Level.Id), OrderByType.Descend);
            var levelsResp = await Bridge.GetAsync(query);
            var videoPath = GetFilePath("sample-video.mp4");
            var levelId = levelsResp.Models.First().Id;

            var deployVideoData = new DeployLevelVideoReq
            {
                LocalPath = videoPath,
                LevelId = levelId,
                DurationSec = 10,
                AllowRemix = true
            };
            var uploadResp = await Bridge.UploadLevelVideoAsync(deployVideoData);

            Debug.Log(JsonConvert.SerializeObject(uploadResp));
        }
    }
}