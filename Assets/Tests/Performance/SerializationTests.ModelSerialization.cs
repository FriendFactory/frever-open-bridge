using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bridge.Models.AsseManager;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using CharacterController = Bridge.Models.AsseManager.CharacterController;
using Event = Bridge.Models.AsseManager.Event;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Tests.Performance
{
    public partial class SerializationTests
    {
         [Test, Performance]
        public void SerializeLevelModelWithProtobuf()
        {
            var testData = GetTestLevelData();

            Measure.Method(() => { _serializer.SerializeProtobuf(testData); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }

        [Test, Performance]
        public void SerializeLevelModelWithJsonNet()
        {
            var testData = GetTestLevelData();

            Measure.Method(() => { _serializer.SerializeToJson(testData); }).WarmupCount(10).MeasurementCount(10)
                .IterationsPerMeasurement(5).Run();
        }
        
        [Test, Performance]
        public void DeserializeLevelModelWithProtobuf()
        {
            var levelModel = GetTestLevelData();

            var bytes = _serializer.SerializeProtobuf(levelModel);
            var filePath = Application.persistentDataPath + "/Tests/Performance/levelModelProtobuf";
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllBytes(filePath, bytes);

            Measure.Method(() =>
            {
                using (var stream = File.OpenRead(filePath))
                {
                    _serializer.DeserializeProtobuf<Level>(stream);
                }
            }).WarmupCount(10).MeasurementCount(10).IterationsPerMeasurement(5).Run();
        }

        [Test, Performance]
        public void DeserializeLevelModelWithJsonNet()
        {
            var levelModel = GetTestLevelData();

            var testDataJson = _serializer.SerializeToJson(levelModel);
            var filePath = Application.persistentDataPath + "/Tests/Performance/levelModelJson";
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, testDataJson);

            Measure.Method(() =>
            {
                var json = File.ReadAllText(filePath);
                _serializer.DeserializeJson<Level>(json);
            }).WarmupCount(10).MeasurementCount(10).IterationsPerMeasurement(5).Run();
        }
        
        private Level GetTestLevelData()
        {
            var lvl = new Level();
            lvl.Id = 1232141;
            for (int i = 0; i < 5; i++)
            {
                var ev = new Event();
                ev.Id = i;
                ev.GroupId = 20;

                var characterController = new CharacterController()
                {
                    Id = i,
                    ControllerSequenceNumber = i
                };
                characterController.CharacterControllerFaceVoice.Add(new CharacterControllerFaceVoice()
                {
                    FaceAnimation = new FaceAnimation()
                    {
                        Files = new List<FileInfo>()
                    },
                    VoiceTrack = new VoiceTrack()
                    {
                        Files = new List<FileInfo>()
                    }
                });

                characterController.CharacterControllerFaceVoice.First().FaceAnimation.Files.Add(
                    new FileInfo()
                    {
                        Version = Guid.NewGuid().ToString()
                    });

                characterController.CharacterControllerFaceVoice.First().VoiceTrack.Files.Add(
                    new FileInfo()
                    {
                        Version = Guid.NewGuid().ToString()
                    });


                ev.CharacterController.Add(characterController);

                ev.SetLocationController.Add(new SetLocationController()
                {
                    SetLocationId = i
                });

                lvl.Event.Add(ev);
            }

            return lvl;
        }
    }
}