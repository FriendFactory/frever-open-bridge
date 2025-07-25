﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class Prop
    {
        public Prop()
        {
            AccountTransactionAndProp = new HashSet<AccountTransactionAndProp>();
            BundleAndProp = new HashSet<BundleAndProp>();
            DeviceTypeAndProp = new HashSet<DeviceTypeAndProp>();
            GroupAndProp = new HashSet<GroupAndProp>();
            LevelTemplateAndProp = new HashSet<LevelTemplateAndProp>();
            PriceruleAndProp = new HashSet<PriceruleAndProp>();
            PropAndBodyAnimation = new HashSet<PropAndBodyAnimation>();
            PropAndSetLocation = new HashSet<PropAndSetLocation>();
            PropController = new HashSet<PropController>();
            QaComments = new HashSet<QaComments>();
            SeasonAndProp = new HashSet<SeasonAndProp>();
            WardrobeAndProp = new HashSet<WardrobeAndProp>();
        }

        public long Id { get; set; }
        public long? AssetStoreInfoId { get; set; }
        public long GroupId { get; set; }
        public long PropCategoryId { get; set; }
        public long PropSubCategoryId { get; set; }
        public long? BrandId { get; set; }
        public long PropWorldSizeId { get; set; }
        public long ReadinessId { get; set; }
        public long? JointPosId { get; set; }
        public long? Size { get; set; }
        public int? PolyCount { get; set; }
        public string Name { get; set; }
        public long UploaderUserId { get; set; }
        public long[] Tags { get; set; }
        public int SortOrder { get; set; }

        public virtual AssetStoreInfo AssetStoreInfo { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Group Group { get; set; }
        public virtual JointPos JointPos { get; set; }
        public virtual PropCategory PropCategory { get; set; }
        public virtual PropSubCategory PropSubCategory { get; set; }
        public virtual PropWorldSize PropWorldSize { get; set; }
        public virtual Readiness Readiness { get; set; }
        public virtual User UploaderUser { get; set; }
        public virtual ICollection<AccountTransactionAndProp> AccountTransactionAndProp { get; set; }
        public virtual ICollection<BundleAndProp> BundleAndProp { get; set; }
        public virtual ICollection<DeviceTypeAndProp> DeviceTypeAndProp { get; set; }
        public virtual ICollection<GroupAndProp> GroupAndProp { get; set; }
        public virtual ICollection<LevelTemplateAndProp> LevelTemplateAndProp { get; set; }
        public virtual ICollection<PriceruleAndProp> PriceruleAndProp { get; set; }
        public virtual ICollection<PropAndBodyAnimation> PropAndBodyAnimation { get; set; }
        public virtual ICollection<PropAndSetLocation> PropAndSetLocation { get; set; }
        public virtual ICollection<PropController> PropController { get; set; }
        public virtual ICollection<QaComments> QaComments { get; set; }
        public virtual ICollection<SeasonAndProp> SeasonAndProp { get; set; }
        public virtual ICollection<WardrobeAndProp> WardrobeAndProp { get; set; }
    }
}