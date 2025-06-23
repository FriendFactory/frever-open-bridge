using System.Collections.Generic;
using System.Linq;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

namespace ApiTests.Events
{
    public class UpdateEventTest : EntityApiTest<Event>
    {
        protected override async void RunTestAsync()
        {
            var levelId = await GetAnyAvailableEntityId<Level>();
            var levelResponse = await Bridge.GetAsync<Level>(levelId);
            var lvl = levelResponse.ResultObject;

            var ev = lvl.Event.First();
            var characterController = ev.CharacterController.First();
            var faceAndVoice = characterController.CharacterControllerFaceVoice.First();
            faceAndVoice.FaceAnimation.Files = new List<FileInfo>
            {
                new FileInfo(GetFilePath(TestFileNames.FACE_ANIMATION), FileType.MainFile)
            };

            faceAndVoice.VoiceTrack.Files = new List<FileInfo>
            {
                new FileInfo(GetFilePath(TestFileNames.VOICE_TRACK), FileType.MainFile)
            };

            var cameraAnimation = ev.CameraController.First().CameraAnimation;
            cameraAnimation.Files = new List<FileInfo>
            {
                new FileInfo(GetFilePath(TestFileNames.CAMERA_ANIMATION), FileType.MainFile)
            };

            var updateResp = await Bridge.UpdateAsync(ev);
            LogResult(updateResp);
        }
    }
}