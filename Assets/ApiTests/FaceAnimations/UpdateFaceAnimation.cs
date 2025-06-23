using System.Collections.Generic;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

namespace ApiTests.FaceAnimations
{
    public class UpdateFaceAnimation : EntityApiTest<FaceAnimation>
    {
        protected override async void RunTestAsync()
        {
            var faceAnimId = await GetAnyAvailableEntityId<FaceAnimation>();

            var faceAnimResp = await Bridge.GetAsync<FaceAnimation>(faceAnimId);
            var faceAnim = faceAnimResp.ResultObject;

            faceAnim.Files = new List<FileInfo>()
                {
                    new FileInfo(GetFilePath(TestFileNames.FACE_ANIMATION), FileType.MainFile)
                };

            var updateResp = await Bridge.UpdateAsync(faceAnim);
            LogResult(updateResp);
        }
    }
}
