using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Bridge.Models.VideoServer;
using Bridge.Modules.Serialization;
using NUnit.Framework;

namespace Tests.Common
{
    public class ProtobufTests 
    {
        private ISerializer _serializer = new Serializer();
        
        [Test]
        public void SerializeSimpleModelViaProtobuf()
        {
            var level = new Level();
            level.Id = 10;
            var bytes = _serializer.SerializeProtobuf(level);
            var deserLevel = _serializer.DeserializeProtobuf<Level>(bytes);
            
            Assert.AreEqual(level.Id, deserLevel.Id);
        }
        
        [Test]
        public void SerializeModelWithEnum_ViaProtobuf()
        {
            var level = new Level();
            level.Id = 10;
            level.Event = new List<Event>()
            {
                new Event()
                {
                    Files = new List<FileInfo>()
                    {
                        new FileInfo()
                        {
                            Extension = FileExtension.Empty
                        }
                    }
                }
            };
            var bytes = _serializer.SerializeProtobuf(level);
            var deserLevel = _serializer.DeserializeProtobuf<Level>(bytes);
            
            Assert.AreEqual(level.Id, deserLevel.Id);
        }
        
        [Test]
        public void SerializeFullLevel_ViaProtobuf()
        {
            var level = new Level();
            level.Id = 10;
            var ev = new Event()
            {
                Files = new List<FileInfo>()
                {
                    new FileInfo()
                    {
                        Extension = FileExtension.Empty
                    }
                },
            };

            ev.CharacterController = new List<CharacterController>();
            ev.CharacterController.Add(new CharacterController()
            {
                CharacterControllerFaceVoice = new List<CharacterControllerFaceVoice>()
                {
                    new CharacterControllerFaceVoice()
                    {
                        FaceAnimation = new FaceAnimation()
                        {
                            Id = 100
                        }
                    }
                }
            });
            
            level.Event = new List<Event>(){ev};
            var bytes = _serializer.SerializeProtobuf(level);
            var deserLevel = _serializer.DeserializeProtobuf<Level>(bytes);
            
            Assert.AreEqual(level.Id, deserLevel.Id);
            Assert.AreEqual(level.Event.First().CharacterController.First().CharacterControllerFaceVoice.First().FaceAnimation.Id, 
                deserLevel.Event.First().CharacterController.First().CharacterControllerFaceVoice.First().FaceAnimation.Id);
        }

        [Test]
        public void Serialize_Comments()
        {
            var commentCount = 3;
            
            var testData = new ResultWithCount<CommentInfo>();
            testData.Count = commentCount;
            testData.Data = new CommentInfo[commentCount];
            for (int i = 0; i < commentCount; i++)
            {
                testData.Data[i] = new CommentInfo()
                {
                    Id = i,
                    GroupId = i,
                    GroupNickname = $"Nickname {i}",
                    Text = "Hello world",
                    VideoId = i,
                    Time = DateTime.UtcNow
                };
            }

            var bytes= _serializer.SerializeProtobuf(testData);
            var result = _serializer.DeserializeProtobuf<ResultWithCount<CommentInfo>>(bytes);

            Assert.IsTrue(result.Data.Length == testData.Data.Length);
        }

        [Test]
        public void TestSerializationOfFloat()
        {
            var modelWithFloat = new VotingResult();
            modelWithFloat.Score = 3.56f;
            modelWithFloat.Place = 1;

            var serialized = _serializer.SerializeProtobuf(modelWithFloat);
            var deserialized = _serializer.DeserializeProtobuf<FloatContainingModel>(serialized);
            Assert.AreEqual(modelWithFloat.Place, deserialized.Place);
            Assert.AreEqual(modelWithFloat.Score, deserialized.Score);
        }
        
        internal class FloatContainingModel
        {
            public int Place { get; set; }
            public float Score  { get; set; }
        }

        public class Comment
        {
            public long Id { get; set; }

            public long VideoId { get; set; }

            public long GroupId { get; set; }

            public DateTime Time { get; set; }

            public string Text { get; set; }
        }
        
        public class CommentInfo: Comment
        {
            public string GroupNickname { get; set; }
        }

        public class ResultWithCount<T>
        {
            public T[] Data { get; set; }

            public int Count { get; set; }
        }
    }
}
