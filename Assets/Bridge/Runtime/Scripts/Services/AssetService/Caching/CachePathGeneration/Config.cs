using System;
using System.Collections.Generic;
using Bridge.ClientServer.ImageGeneration;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Template;
using Bridge.Models.AsseManager;
using Bridge.Models.ClientServer;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.ClientServer.CreatorScore;
using Bridge.Models.ClientServer.Crews;
using Bridge.Models.ClientServer.Gamification;
using Bridge.Models.ClientServer.StartPack.Metadata;
using Bridge.Models.ClientServer.ThemeCollection;
using Bridge.Models.Common.Files;
using Bridge.Models.VideoServer;
using Bridge.Services.Advertising;
using Bridge.VideoServer;
using BodyAnimation = Bridge.Models.AsseManager.BodyAnimation;
using CameraAnimationTemplate = Bridge.Models.AsseManager.CameraAnimationTemplate;
using CameraAnimationType = Bridge.Models.AsseManager.CameraAnimationType;
using CharacterSpawnPositionFormation = Bridge.Models.AsseManager.CharacterSpawnPositionFormation;
using SongInfo = Bridge.Models.ClientServer.Assets.SongInfo;
using WardrobeCategory = Bridge.Models.AsseManager.WardrobeCategory;
using WardrobeCategoryType = Bridge.Models.AsseManager.WardrobeCategoryType;

namespace Bridge.Services.AssetService.Caching.CachePathGeneration
{
    internal static class Config
    {
        private static readonly AssetSetting CharacterSettings = new(
            
            "Character",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting EventSettings = new(
            "Event",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting SetLocationSettings = new(
            "SetLocation",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512),
            new ThumbnailSettings(FileExtension.Png, Resolution._1600x900));

        private static readonly AssetSetting VfxSettings = new(
            "Vfx",
            new ThumbnailSettings(FileExtension.Gif, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Gif, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Gif, Resolution._512x512),
            new AssetBundleSettings());

        private static readonly AssetSetting SongSettings = new(
            "Song",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512),
            new AudioFileSettings(new[] { FileExtension.Mp3, FileExtension.Ogg }));

        private static readonly AssetSetting VoiceTrackSettings =
            new("VoiceTrack", new AudioFileSettings(FileExtension.Wav));

        private static readonly AssetSetting UserSoundSettings =
            new("UserSound", new AudioFileSettings(FileExtension.Mp3));

        private static readonly AssetSetting FaceAnimationSettings =
            new("FaceAnimation", new TextFileSettings("FaceAnimation"));

        private static readonly AssetSetting UmaBundleSettings = new("UmaBundle", new AssetBundleSettings());
        
        private static readonly AssetSetting CharacterBackedViewSettings =
            new("CharacterBakedView", new AssetBundleSettings());

        private static readonly AssetSetting BodyAnimationSettings = new(
            "BodyAnimation",
            new ThumbnailSettings(FileExtension.Gif, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Gif, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Gif, Resolution._512x512),
            new AssetBundleSettings());

        private static readonly AssetSetting CameraAnimationTemplateSettings = new(
            "CameraAnimationTemplate",
            new ThumbnailSettings(FileExtension.Gif, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Gif, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Gif, Resolution._512x512));

        private static readonly AssetSetting VoiceFilterSettings = new(
            "VoiceFilter",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting CameraAnimationSettings =
            new("CameraAnimation", new TextFileSettings("CameraAnimation"));

        private static readonly AssetSetting CameraAnimationTypeSettings = new(
            "CameraAnimationType",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512)
        );

        private static readonly AssetSetting WardrobeSettings = new(
            "Wardrobe",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting WardrobeCategorySettings = new(
            "WardrobeCategory",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting SetLocationBundleSettings =
            new("SetLocationBundle",new AssetBundleSettings());

        private static readonly AssetSetting CharacterSpawnPositionSettings = new(
            "CharacterSpawnPosition",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512),
            new ThumbnailSettings(FileExtension.Png, Resolution._1600x900));

        private static readonly AssetSetting WardrobeCategoryTypeSettings = new(
            "WardrobeCategoryType",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting CharacterSpawnPositionFormationSettings = new(
            "CharacterSpawnPositionFormation",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting CameraFilterSettings = new(
            "CameraFilter",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting CameraFilterVariantSettings = new(
            "CameraFilterVariant",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512),
            new AssetBundleSettings());

        private static readonly AssetSetting OutfitSettings = new(
            "Outfit",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting VideoClipSettings = new("VideoClip", new VideoMainFileSettings());

        private static readonly AssetSetting PhotoSettings =
            new("Photo",
                new ImageSettings(FileType.MainFile, new[] { FileExtension.Jpg, FileExtension.Jpeg, FileExtension.Png }, null, "Image"));

        private static readonly AssetSetting MarketingScreenshotSettings = new("MarketingScreenshot", new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting RewardSettings = new("SeasonReward",
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting TemplateSettings = new("Template");

        private static readonly AssetSetting InAppProductOffer = new(nameof(InAppProductOffer),
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512),
            new ThumbnailSettings(FileExtension.Png, Resolution._1024x1024));
        
        private static readonly AssetSetting LootBox = new(nameof(LootBox),
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));
        
        private static readonly AssetSetting PromotedSongSettings = new(nameof(PromotedSong),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting Crew = new("Crew",
            new ImageSettings(FileType.MainFile, new[] {FileExtension.Jpeg, FileExtension.Jpg}, null, "Crew"),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));
        
        private static readonly AssetSetting ChatMessageSettings = new("ChatMessage",
            new ImageSettings(FileType.MainFile, new[] {FileExtension.Jpeg, FileExtension.Jpg}, null, "Image"),
            new AudioFileSettings(new [] { FileExtension.Mp3, FileExtension.Ogg, FileExtension.Wav}),
            new VideoMainFileSettings(),
            new TextFileSettings("Text"));

        private static readonly AssetSetting MovementType = new("MovementType",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128));
        
        private static readonly AssetSetting Emotion = new("Emotion",
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128));
       
        private static readonly AssetSetting ThemeCollection = new("ThemeCollection",
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));

        private static readonly AssetSetting SetLocationBackground = new(nameof(SetLocationBackground),
            new ThumbnailSettings(FileExtension.Jpg, Resolution._128x128),
            new ImageSettings(FileType.MainFile, new[] { FileExtension.Jpg }, null, nameof(SetLocationBackground)));

        private static readonly AssetSetting SetLocationBackgroundSettings = new(nameof(SetLocationBackgroundSettings), 
            new ThumbnailSettings(FileExtension.Jpg, Resolution._128x128),
            new ImageSettings(FileType.MainFile, new[] { FileExtension.Png }, null, nameof(SetLocationBackground)));

        private static readonly AssetSetting UniverseSettings = new(nameof(Universe),
            new ThumbnailSettings(FileExtension.Png, Resolution._64x64),
            new ThumbnailSettings(FileExtension.Png, Resolution._128x128),
            new ThumbnailSettings(FileExtension.Png, Resolution._512x512));
        
        private static readonly AssetSetting WatermarkSettings = new(nameof(Watermark),
            new ImageSettings(FileType.MainFile, new[] { FileExtension.Png }, null, nameof(Watermark), true));

        private static readonly AssetSetting TransformationStyleSettings = new(nameof(TransformationStyle),
            new ThumbnailSettings(FileExtension.Png, Resolution._256x256));
        
        private static readonly AssetSetting MakeUpSettings = new(nameof(MakeUp),
            new ThumbnailSettings(FileExtension.Jpeg, Resolution._128x128));
        
        private static readonly AssetSetting GeneratedImageSettings = new("Picture",
            new ImageSettings(FileType.MainFile,new []{FileExtension.Jpeg}, null, "Image"));
        
        public static readonly Dictionary<Type, AssetSetting> Settings = new()
        {
            { typeof(Character), CharacterSettings },
            { typeof(CharacterBakedView), CharacterBackedViewSettings },
            { typeof(BakedView), CharacterBackedViewSettings },
            { typeof(CharacterInfo), CharacterSettings },
            { typeof(CharacterFullInfo), CharacterSettings },
            { typeof(Event), EventSettings },
            { typeof(EventFullInfo), EventSettings },
            { typeof(SetLocation), SetLocationSettings },
            { typeof(SetLocationFullInfo), SetLocationSettings },
            { typeof(Vfx), VfxSettings },
            { typeof(VfxFullInfo), VfxSettings },
            { typeof(VfxInfo), VfxSettings },
            { typeof(Song), SongSettings },
            { typeof(SongInfo), SongSettings },
            { typeof(SongFullInfo), SongSettings },
            { typeof(VoiceTrack), VoiceTrackSettings },
            { typeof(VoiceTrackFullInfo), VoiceTrackSettings },
            { typeof(UserSound), UserSoundSettings },
            { typeof(UserSoundFullInfo), UserSoundSettings },
            { typeof(FaceAnimation), FaceAnimationSettings },
            { typeof(FaceAnimationFullInfo), FaceAnimationSettings },
            { typeof(UmaBundle), UmaBundleSettings },
            { typeof(UmaBundleFullInfo), UmaBundleSettings },
            { typeof(BodyAnimation), BodyAnimationSettings },
            { typeof(BodyAnimationInfo), BodyAnimationSettings },
            { typeof(BodyAnimationFullInfo), BodyAnimationSettings },
            { typeof(CameraAnimationTemplate), CameraAnimationTemplateSettings },
            { typeof(Models.ClientServer.StartPack.Metadata.CameraAnimationTemplate), CameraAnimationTemplateSettings },
            { typeof(VoiceFilter), VoiceFilterSettings },
            { typeof(VoiceFilterFullInfo), VoiceFilterSettings },
            { typeof(CameraAnimation), CameraAnimationSettings },
            { typeof(CameraAnimationFullInfo), CameraAnimationSettings },
            { typeof(CameraAnimationType), CameraAnimationTypeSettings },
            { typeof(Models.ClientServer.StartPack.Metadata.CameraAnimationType), CameraAnimationTypeSettings },
            { typeof(Wardrobe), WardrobeSettings },
            { typeof(WardrobeFullInfo), WardrobeSettings },
            { typeof(WardrobeShortInfo), WardrobeSettings },
            { typeof(WardrobeCategory), WardrobeCategorySettings },
            { typeof(Models.ClientServer.StartPack.Metadata.WardrobeCategory), WardrobeCategorySettings },
            { typeof(SetLocationBundle), SetLocationBundleSettings },
            { typeof(SetLocationBundleInfo), SetLocationBundleSettings },
            { typeof(CharacterSpawnPosition), CharacterSpawnPositionSettings },
            { typeof(CharacterSpawnPositionInfo), CharacterSpawnPositionSettings },
            { typeof(WardrobeCategoryType), WardrobeCategoryTypeSettings },
            { typeof(Models.ClientServer.StartPack.Metadata.WardrobeCategoryType), WardrobeCategoryTypeSettings },
            { typeof(CharacterSpawnPositionFormation), CharacterSpawnPositionFormationSettings },
            {
                typeof(Models.ClientServer.StartPack.Metadata.CharacterSpawnPositionFormation),
                CharacterSpawnPositionFormationSettings
            },
            { typeof(CameraFilter), CameraFilterSettings },
            { typeof(CameraFilterInfo), CameraFilterSettings },
            { typeof(CameraFilterVariant), CameraFilterVariantSettings },
            { typeof(CameraFilterVariantInfo), CameraFilterVariantSettings },
            { typeof(Outfit), OutfitSettings },
            { typeof(OutfitShortInfo), OutfitSettings },
            { typeof(OutfitFullInfo), OutfitSettings },
            { typeof(VideoClip), VideoClipSettings },
            { typeof(VideoClipFullInfo), VideoClipSettings },
            { typeof(Photo), PhotoSettings },
            { typeof(PhotoFullInfo), PhotoSettings },
            { typeof(Template), TemplateSettings },
            { typeof(TemplateChallenge), TemplateSettings },
            { typeof(TemplateInfo), TemplateSettings },
            { typeof(MarketingScreenshot), MarketingScreenshotSettings },
            { typeof(PastSeasonAssetReward), RewardSettings },
            { typeof(SeasonReward), RewardSettings },
            { typeof(CreatorBadgeReward), RewardSettings},
            { typeof(InAppProductOffer), InAppProductOffer },
            { typeof(LootBox), LootBox },
            { typeof(PromotedSong), PromotedSongSettings },
            { typeof(TrendingUserSound), UserSoundSettings },
            { typeof(CrewModel), Crew },
            { typeof(CrewShortInfo), Crew },
            { typeof(ChatMessage), ChatMessageSettings },
            { typeof(Bridge.Models.ClientServer.StartPack.Metadata.MovementType), MovementType },
            { typeof(Emotion), Emotion},
            { typeof(ThemeCollectionInfo), ThemeCollection },
            { typeof(SetLocationBackground), SetLocationBackground },
            { typeof(CrewTopInfo), Crew },
            { typeof(SetLocationBackgroundSettings), SetLocationBackgroundSettings },
            { typeof(Universe), UniverseSettings},
            { typeof(Watermark), WatermarkSettings },
            { typeof(TransformationStyle), TransformationStyleSettings },
            { typeof(MakeUp), MakeUpSettings },
            { typeof(GeneratedImage), GeneratedImageSettings },
        };
    }
}