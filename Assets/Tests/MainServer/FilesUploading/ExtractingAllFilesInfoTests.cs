using System.Collections.Generic;
using System.Linq;
using Bridge.Models.AsseManager;
using Bridge.Models.AsseManager.Extensions.FilesContainable;
using Bridge.Models.Common.Files;
using NUnit.Framework;

namespace Tests.MainServer.FilesUploading
{
    public class ExtractingAllFilesInfoTests
    {
        [Test]
        public void ExtractModelsFromLevelModel_ShouldExtractAllModels()
        {
            var level = new Level();

            var faceAndVoice = new CharacterControllerFaceVoice();
            faceAndVoice.VoiceTrack = new VoiceTrack()
            {
                Files = new List<FileInfo>()//1
            };
            faceAndVoice.FaceAnimation = new FaceAnimation()
            {
                Files = new List<FileInfo>() //2
            };

            var characterController = new CharacterController();
            characterController.CharacterControllerFaceVoice.Add(faceAndVoice);

            var ev = new Event();
            ev.CharacterController.Add(characterController);

            var cameraController = new CameraController();
            cameraController.CameraAnimation = new CameraAnimation()
            {
                Files = new List<FileInfo>()//3
            };

            ev.CameraController.Add(cameraController);

            level.Event.Add(ev);
            
            var extracted = level.ExtractAllModelWithFiles();

            Assert.IsTrue(extracted.Count == 3);
            Assert.IsTrue(extracted.GroupBy(x=>x.GetHashCode()).Count()==3);
        }
    }
}
