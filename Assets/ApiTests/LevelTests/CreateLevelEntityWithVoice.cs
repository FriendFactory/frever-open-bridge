using System.Collections.Generic;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using BodyAnimation = Bridge.Models.AsseManager.BodyAnimation;
using CharacterController = Bridge.Models.AsseManager.CharacterController;
using Event = Bridge.Models.AsseManager.Event;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace ApiTests.Levels
{
    public class CreateLevelEntityWithVoice : LevelEntityApiTest
    {
        protected override async void RunTestAsync()
        {
            var lvl = new Level
            {
                LevelTemplateId = 1,
                LanguageId = 1,
                VerticalCategoryId = 1,
                OriginalGroupId = Bridge.Profile.GroupId
            };
            lvl.LanguageId = 1;

            var newEvent = new Event();
            newEvent.LevelSequence = 1;
            newEvent.CameraController.Add(new CameraController()
            {
                ActivationCue = 0,
                EndCue = 1000,
                LookAtIndex = 1,
                SpawnIndex = 1,
                CameraNoiseSettingsIndex = 1,
                ControllerSequenceNumber = 1,
                CameraAnimation = new CameraAnimation()
                {
                    Files = new List<FileInfo>() {new FileInfo(GetFilePath(TestFileNames.CAMERA_ANIMATION), FileType.MainFile)}
                },
                CameraAnimationTemplateId = await GetAnyAvailableEntityId<CameraAnimationTemplate>()
            });

            var setLocationId = await GetAnyAvailableEntityId<SetLocation>();
            newEvent.SetLocationController.Add(new SetLocationController()
            {
                SetLocationId = setLocationId,
                WeatherId = await GetAnyAvailableEntityId<Weather>(),
                ControllerSequenceNumber = 1
            });

            var characterController = new CharacterController()
            {
                CharacterId = await GetAnyAvailableEntityId<Character>(),
                ControllerSequenceNumber = 1,
                CharacterControllerFaceVoice = new List<CharacterControllerFaceVoice>()
                    {
                        new CharacterControllerFaceVoice()
                        {
                            FaceAnimation = new FaceAnimation()
                            {
                                FaceAnimationCategoryId = await GetAnyAvailableEntityId<FaceAnimationCategory>(),
                                Duration = 100,
                                Files = new List<FileInfo>()
                                {
                                    new FileInfo(GetFilePath(TestFileNames.FACE_ANIMATION), FileType.MainFile)
                                }
                            },
                            VoiceTrack = new VoiceTrack()
                            {
                                LanguageId = 1,
                                Duration = 100,
                                Files = new List<FileInfo>() {new FileInfo(GetFilePath(TestFileNames.VOICE_TRACK), FileType.MainFile)}
                            },
                            ControllerSequenceNumber = 1
                        }
                    },
                CharacterControllerUmaRecipe = new List<CharacterControllerUmaRecipe>()
                    {
                        new CharacterControllerUmaRecipe()
                        {
                            UmaRecipeId = await GetAnyAvailableEntityId<UmaRecipe>(),
                            ControllerSequenceNumber = 1
                        }
                    },
                CharacterControllerBodyAnimation = new List<CharacterControllerBodyAnimation>()
                    {
                        new CharacterControllerBodyAnimation()
                        {
                            PrimaryBodyAnimationId = await GetAnyAvailableEntityId<BodyAnimation>(),
                            AnimationSpeed = 100,
                            ControllerSequenceNumber = 1
                        }
                    }
            };

            newEvent.CharacterController.Add(characterController);
            var characterControllerCopy =
                JsonConvert.DeserializeObject<CharacterController>(JsonConvert.SerializeObject(characterController));
            newEvent.CharacterController.Add(characterControllerCopy);
            characterControllerCopy.ControllerSequenceNumber++;

            newEvent.CharacterSpawnPositionId = await GetAnySpawnPositionId(setLocationId);
            newEvent.Files = new List<FileInfo>();
            newEvent.Files.Add(new FileInfo(GetFilePath(ApiTests.TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._128x128));
            newEvent.Files.Add(new FileInfo(GetFilePath(ApiTests.TestFileNames.THUMBNAIL_PNG_2), FileType.Thumbnail, Resolution._512x512));
            
            lvl.Event.Add(newEvent);

            var res = await Bridge.PostAsync(lvl);
            LogResult(res);
        }

       
    }
}