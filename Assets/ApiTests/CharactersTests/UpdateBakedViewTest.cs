using System.Collections.Generic;
using System.Linq;
using Bridge.Models.AdminService;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace ApiTests.CharactersTests
{
    internal sealed class UpdateBakedViewTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private long _characterId;
        [SerializeField] private long _outfitId;
        
        protected override async void RunTestAsync()
        {
            var characters = await Bridge.GetCharactersAdminAccessLevel(new [] { _characterId });
            var character = characters.Models.First();
            var bv = character.BakedViews.First();
            
            var model = new CharacterBakedViewDto();
            model.Files = new List<FileInfo>();
            var bundlePathIOS = GetFilePath(TestFileNames.BAKED_VIEW_IOS);
            var bundlePathAndroid = GetFilePath(TestFileNames.BAKED_VIEW_ANDROID);
            model.Files.Add(new FileInfo(bundlePathIOS, FileType.MainFile, Platform.iOS)
            {
                Extension = FileExtension.Empty
            });
            model.Files.Add(new FileInfo(bundlePathAndroid, FileType.MainFile, Platform.Android)
            {
                Extension = FileExtension.Empty
            });
            foreach (var file in model.Files)
            {
                file.UnityVersion = Application.unityVersion;
            }

            model.CharacterId = _characterId;
            model.CharacterVersion = character.Version;
            model.IsValid = true;
            model.ReadinessId = 2;
            if (_outfitId > 0)
            {
                model.OutfitId = _outfitId;
            }

            var resp = await Bridge.UpdateBakedView(bv.Id, model);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}