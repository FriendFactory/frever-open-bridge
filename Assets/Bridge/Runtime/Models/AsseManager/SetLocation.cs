﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class SetLocation
    {
        public SetLocation()
        {
            AccountTransactionAndSetLocation = new HashSet<AccountTransactionAndSetLocation>();
            BundleAndSetLocation = new HashSet<BundleAndSetLocation>();
            DeviceTypeAndSetLocation = new HashSet<DeviceTypeAndSetLocation>();
            GroupAndSetLocation = new HashSet<GroupAndSetLocation>();
            LevelTemplateAndSetLocation = new HashSet<LevelTemplateAndSetLocation>();
            PriceruleAndSetLocation = new HashSet<PriceruleAndSetLocation>();
            PropAndSetLocation = new HashSet<PropAndSetLocation>();
            QaComments = new HashSet<QaComments>();
            SeasonAndSetLocation = new HashSet<SeasonAndSetLocation>();
            SetLocationAndCharacterSpawnPosition = new HashSet<SetLocationAndCharacterSpawnPosition>();
            SetLocationController = new HashSet<SetLocationController>();
            VfxPositionGroup = new HashSet<VfxPositionGroup>();
            WardrobeAndSetLocation = new HashSet<WardrobeAndSetLocation>();
        }

        public long Id { get; set; }
        public long? AssetStoreInfoId { get; set; }
        public int? SortOrderByMood { get; set; }
        public int? SortOrderByCategory { get; set; }
        public long SetLocationCategoryId { get; set; }
        public long SetLocationMoodId { get; set; }
        public long SetLocationTemplateId { get; set; }
        public long GroupId { get; set; }
        public bool AllowPhoto { get; set; }
        public bool AllowVideo { get; set; }
        public int VfxSpawnPositionIndexMax { get; set; }
        public long GeoReferenceId { get; set; }
        public long WeatherId { get; set; }
        public long ReadinessId { get; set; }
        public bool CharacterLocomotionAllowed { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Name { get; set; }
        public long UploaderUserId { get; set; }
        public long? VfxTypeId { get; set; }
        public long PipelineId { get; set; }
        public long SetLocationBundleId { get; set; }
        public long SetLocationSubCategoryId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsStartPackMember { get; set; }
        public long? AssetTierId { get; set; }
        public long[] Tags { get; set; }
        public string FilesInfo { get; set; }

        public virtual AssetTier AssetTier { get; set; }
        public virtual AssetStoreInfo AssetStoreInfo { get; set; }
        public virtual GeoReference GeoReference { get; set; }
        public virtual Group Group { get; set; }
        public virtual Pipeline Pipeline { get; set; }
        public virtual Readiness Readiness { get; set; }
        public virtual SetLocationBundle SetLocationBundle { get; set; }
        public virtual SetLocationCategory SetLocationCategory { get; set; }
        public virtual SetLocationMood SetLocationMood { get; set; }
        public virtual SetLocationSubCategory SetLocationSubCategory { get; set; }
        public virtual SetLocationTemplate SetLocationTemplate { get; set; }
        public virtual User UploaderUser { get; set; }
        public virtual VfxType VfxType { get; set; }
        public virtual Weather Weather { get; set; }
        public virtual ICollection<AccountTransactionAndSetLocation> AccountTransactionAndSetLocation { get; set; }
        public virtual ICollection<BundleAndSetLocation> BundleAndSetLocation { get; set; }
        public virtual ICollection<DeviceTypeAndSetLocation> DeviceTypeAndSetLocation { get; set; }
        public virtual ICollection<GroupAndSetLocation> GroupAndSetLocation { get; set; }
        public virtual ICollection<LevelTemplateAndSetLocation> LevelTemplateAndSetLocation { get; set; }
        public virtual ICollection<PriceruleAndSetLocation> PriceruleAndSetLocation { get; set; }
        public virtual ICollection<PropAndSetLocation> PropAndSetLocation { get; set; }
        public virtual ICollection<QaComments> QaComments { get; set; }
        public virtual ICollection<SeasonAndSetLocation> SeasonAndSetLocation { get; set; }
        public virtual ICollection<SetLocationAndCharacterSpawnPosition> SetLocationAndCharacterSpawnPosition { get; set; }
        public virtual ICollection<SetLocationController> SetLocationController { get; set; }
        public virtual ICollection<VfxPositionGroup> VfxPositionGroup { get; set; }
        public virtual ICollection<WardrobeAndSetLocation> WardrobeAndSetLocation { get; set; }
    }
}