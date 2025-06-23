using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;
using CharacterController = Bridge.Models.AsseManager.CharacterController;
using Event = Bridge.Models.AsseManager.Event;

namespace Bridge.Examples
{
    /// <summary>
    /// It allows to send optimized query to update entities
    /// You need provide origin object and modified. Difference will be send to server
    /// </summary>
    public class OptimizedUpdateExample: MonoBehaviour
    {
        async Task Start()
        {
            //save origin(came for server) model somewhere
            var origin = GetTestLevel();

            var modififed = Copy(origin);
            //then change copy
            modififed.Event.Add(new Event());
            modififed.LanguageId += 1;
            modififed.Event.First().LevelSequence += 1;

            var req = new DifferenceDeepUpdateReq<Level>(origin, modififed);
            var bridge = new ServerBridge();
            await bridge.UpdateAsync(req);
        }

        private Level GetTestLevel()
        {
            var lvl = new Level();
            lvl.Id = 1;
            lvl.LevelTemplateId = 1;
            lvl.GroupId = 1;
            lvl.Event = new List<Event>();
            lvl.Event.Add(new Event()
            {
                Id = 1,
                CharacterController = new List<CharacterController>()
            });

            lvl.Event.First().CharacterController.Add(new CharacterController()
            {
                Id = 1,
                Character = new Character()
                {
                    Id = 10,
                    CharacterStyleId = 1
                }
            });

            lvl.Event.Add(new Event()
            {
                Id = 2,
                CharacterController = new List<CharacterController>()
            });

            lvl.Event.Add(new Event()
            {
                Id = 3,
                CharacterController = new List<CharacterController>()
            });

            return lvl;
        }

        private T Copy<T>(T target)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(target));
        }
    }
}