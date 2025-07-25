﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class AssetStoreInfo
    {
        public AssetStoreInfo()
        {
            Autoclip = new HashSet<Autoclip>();
            BodyAnimation = new HashSet<AsseManager.BodyAnimation>();
            Bundle = new HashSet<Bundle>();
            CameraFilter = new HashSet<CameraFilter>();
            Character = new HashSet<Character>();
            FaceAnimation = new HashSet<FaceAnimation>();
            Outfit = new HashSet<Outfit>();
            Photo = new HashSet<Photo>();
            Prop = new HashSet<Prop>();
            Score = new HashSet<Score>();
            SetLocation = new HashSet<SetLocation>();
            Sfx = new HashSet<Sfx>();
            Song = new HashSet<Song>();
            Sticker = new HashSet<Sticker>();
            Vfx = new HashSet<Vfx>();
            VideoClip = new HashSet<VideoClip>();
            VoiceFilter = new HashSet<VoiceFilter>();
            VoiceTrack = new HashSet<VoiceTrack>();
            Wardrobe = new HashSet<Wardrobe>();
        }

        public long Id { get; set; }
        public long AssetTypeId { get; set; }
        public string DisplayName { get; set; }
        public int[] DescriptionKeywords { get; set; }
        public int SortOrder { get; set; }
        public int MarketingScreenshotCount { get; set; }
        public long Price { get; set; }
        public int? RemainingQuota { get; set; }
        public long ProductionCost { get; set; }
        public long? PayoutContractId { get; set; }
        public long CreatorSourceId { get; set; }

        public virtual AssetType AssetType { get; set; }
        public virtual CreatorSource CreatorSource { get; set; }
        public virtual PayoutContract PayoutContract { get; set; }
        public virtual ICollection<Autoclip> Autoclip { get; set; }
        public virtual ICollection<AsseManager.BodyAnimation> BodyAnimation { get; set; }
        public virtual ICollection<Bundle> Bundle { get; set; }
        public virtual ICollection<CameraFilter> CameraFilter { get; set; }
        public virtual ICollection<Character> Character { get; set; }
        public virtual ICollection<FaceAnimation> FaceAnimation { get; set; }
        public virtual ICollection<Outfit> Outfit { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual ICollection<Prop> Prop { get; set; }
        public virtual ICollection<Score> Score { get; set; }
        public virtual ICollection<SetLocation> SetLocation { get; set; }
        public virtual ICollection<Sfx> Sfx { get; set; }
        public virtual ICollection<Song> Song { get; set; }
        public virtual ICollection<Sticker> Sticker { get; set; }
        public virtual ICollection<Vfx> Vfx { get; set; }
        public virtual ICollection<VideoClip> VideoClip { get; set; }
        public virtual ICollection<VoiceFilter> VoiceFilter { get; set; }
        public virtual ICollection<VoiceTrack> VoiceTrack { get; set; }
        public virtual ICollection<Wardrobe> Wardrobe { get; set; }
    }
}