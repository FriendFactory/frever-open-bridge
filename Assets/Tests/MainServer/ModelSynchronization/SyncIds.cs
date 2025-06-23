using System.Collections.Generic;
using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using NUnit.Framework;
using Event = Bridge.Models.AsseManager.Event;

namespace Tests.MainServer.ModelSynchronization
{
    public class SyncIds
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SyncIdsSimplePasses()
        {
            var source = new Level();
            source.Id = 1;
            source.Language = new Language()
            {
                Id = 2
            };

            var dest = new Level();
            dest.Language = new Language();

            var sync = new ModelDataSynchronizer();
            sync.Sync(source,dest);

            Assert.AreEqual(source.Id, dest.Id);
            Assert.AreEqual(source.Language.Id, dest.Language.Id);
        }

        [Test]
        public void SyncInArrays()
        {
            var source = new Level();
            source.Event = new List<Event>();
            for (int i = 0; i < 5; i++)
            {
                var ev = new Event();
                ev.Id = i + 1;
                source.Event.Add(ev);
            }

            var dest = new Level();
            for (int i = 0; i < 5; i++)
            {
                var ev = new Event();
                dest.Event.Add(ev);
            }

            var sync = new ModelDataSynchronizer();
            sync.Sync(source,dest);
            for (int i = 0; i < dest.Event.Count; i++)
            {
                var ev = dest.Event.ElementAt(i);
                Assert.AreEqual(ev.Id, i+1);
            }
        }

        [Test]
        public void PreventSyncPreviousExistedId()
        {
            var source = new Level();
            source.Group = new Group()
            {
                Id = 10
            };

            var dest = new Level();
            dest.Group = new Group()
            {
                Id = -10
            };

            var sync = new ModelDataSynchronizer();
            sync.Sync(source,dest);

            Assert.AreEqual(dest.Group.Id, -10);
        }

        [Test]
        public void SyncForeignKeys()
        {
            var source = new Level();
            source.Group = new Group()
            {
                Id = 10
            };

            var dest = new Level();
            
            var sync = new ModelDataSynchronizer();
            sync.Sync(source, dest);

            Assert.AreEqual(source.Group.Id, dest.GroupId);
        }

        [Test]
        public void SyncForeignKeyBasedFromNavigationFieldInArray()
        {
            var source = new Level();
            source.Id = 10;
            source.Event = new List<Event>();
            source.Event.Add(new Event()
            {
                Id = 11
            });

            var dest = new Level();
            dest.Event = new List<Event>();
            dest.Event.Add(new Event());
            
            var sync = new ModelDataSynchronizer();
            sync.Sync(source,dest);

            Assert.AreEqual(source.Id, dest.Event.First().LevelId);
        }

        [Test]
        public void CreateDeepStructure_Level()
        {
            var source = new Level();
            source.Id = 9;
            source.Event = new List<Event>();
            source.Event.Add(new Event()
            {
                Id = 10,
                CharacterController = new List<CharacterController>()
                {
                    new CharacterController()
                    {
                        Id = 11
                    }
                }
            });

            var dest = new Level();
            dest.Event = new List<Event>();
            dest.Event.Add(new Event()
            {
                CharacterController = new List<CharacterController>()
                {
                    new CharacterController()
                }
            });

            var sync = new ModelDataSynchronizer();
            sync.Sync(source,dest);

            Assert.AreEqual(source.Id, dest.Id);
            Assert.AreEqual(source.Event.First().Id, dest.Event.First().Id);
            Assert.AreEqual(source.Id, dest.Event.First().LevelId);
            Assert.AreEqual(source.Event.First().CharacterController.First().Id,
                            dest.Event.First().CharacterController.First().Id);
            Assert.AreEqual(source.Event.First().Id, dest.Event.First().CharacterController.First().EventId);
        }

        [Test]
        public void SyncFileInfo()
        {
            var source = new FaceAnimation();
            
            var fileInfo = new FileInfo();
            fileInfo.TagAsSyncedWithServer();
            fileInfo.Version = "1";
            source.Files = new List<FileInfo>();
            source.Files.Add(fileInfo);

            var dest = JsonConvert.DeserializeObject<FaceAnimation>(JsonConvert.SerializeObject(source));
            dest.Files.First().TagAsModified();
            dest.Files.First().Version = "0";

            var sync = new ModelDataSynchronizer(); 
            sync.Sync(source, dest);
            Assert.IsTrue(dest.Files.First().Version == "1");
        }
    }
}
