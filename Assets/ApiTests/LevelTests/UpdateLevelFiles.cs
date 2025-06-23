using System.Collections.Generic;
using System.Linq;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

namespace ApiTests.Levels
{
    public class UpdateLevelFiles: EntityApiTest<Level>
    {
        protected override async void RunTestAsync()
        {
            var anyId = await GetAnyAvailableEntityId<Level>();
            var resp = await Bridge.GetAsync<Level>(anyId);

            var lvl = resp.ResultObject;
            var ev= lvl.Event.First();
            var faceAndVoice = ev.CharacterController.First().CharacterControllerFaceVoice.First();
            faceAndVoice.FaceAnimation.Files = new List<FileInfo>() {new FileInfo(GetFilePath("FaceAnimation.txt"), FileType.MainFile)};

            faceAndVoice.FaceAnimation.Files = new List<FileInfo>()
            {
                new FileInfo(GetFilePath(TestFileNames.FACE_ANIMATION), FileType.MainFile)
            };

            var res = await Bridge.UpdateAsync(lvl);
            LogResult(res);
            
        }
    }
}