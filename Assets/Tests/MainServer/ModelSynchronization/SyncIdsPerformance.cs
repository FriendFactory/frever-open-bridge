using System.Collections.Generic;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Tests.MainServer.ModelSynchronization
{
    public class SyncIdsPerformance
    {
        [Test]
        [Performance]
        public void Test()
        {
            var sourceLevel = CreateLevel(1);
            var destLevel = CreateLevel(0);

            Measure.Method(()=> SyncLevelDeepModel(sourceLevel, destLevel))
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        private void SyncLevelDeepModel(Level source, Level dest)
        {
            var sync = new ModelDataSynchronizer();
            sync.Sync(source, dest);
        }

        private Level CreateLevel(long id)
        {
            var lvl = new Level();
            lvl.Id = id;
            lvl.Event = new List<Event>();
            lvl.Event.Add(CreateEvent(id));
            return lvl;
        }

        private Event CreateEvent(long id)
        {
            var ev = new Event();
            ev.Id = id;
            ev.CharacterController = new List<CharacterController>();
            ev.CharacterController.Add(new CharacterController()
            {
                Id = id,
                CharacterControllerFaceVoice = new List<CharacterControllerFaceVoice>()
                {
                    new CharacterControllerFaceVoice()
                    {
                        Id = id, 
                        FaceAnimation = new FaceAnimation()
                        {
                            Id = id
                        },
                        VoiceTrack = new VoiceTrack()
                        {
                            Id = id
                        }
                    }
                }
            });

            ev.SetLocationController = new List<SetLocationController>();
            ev.SetLocationController.Add(new SetLocationController()
            {
                Id = id
            });

            ev.MusicController = new List<MusicController>();
            ev.MusicController.Add(new MusicController()
            {
                Id = id
            });

            ev.VfxController = new List<VfxController>();
            ev.VfxController.Add(new VfxController()
            {
                Id = id
            });

            return ev;
        }
    }
}
