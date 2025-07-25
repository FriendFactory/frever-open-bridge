﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class CameraFilter
    {
        public CameraFilter()
        {
            AccountTransactionAndCameraFilter = new HashSet<AccountTransactionAndCameraFilter>();
            BundleAndCameraFilter = new HashSet<BundleAndCameraFilter>();
            DeviceTypeAndCameraFilter = new HashSet<DeviceTypeAndCameraFilter>();
            GroupAndCameraFilter = new HashSet<GroupAndCameraFilter>();
            PriceruleAndCameraFilter = new HashSet<PriceruleAndCameraFilter>();
            QaComments = new HashSet<QaComments>();
            SeasonAndCameraFilter = new HashSet<SeasonAndCameraFilter>();
            WardrobeAndCameraFilter = new HashSet<WardrobeAndCameraFilter>();
            CameraFilterVariant = new HashSet<CameraFilterVariant>();
        }

        public long Id { get; set; }
        public long? AssetStoreInfoId { get; set; }
        public long GroupId { get; set; }
        public long CameraFilterCategoryId { get; set; }
        public long LensFilterCategoryId { get; set; }
        public long ReadinessId { get; set; }
        public long ColorFilterCategoryId { get; set; }
        public long UploaderUserId { get; set; }
        public long[] Tags { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public long CameraFilterSubCategoryId { get; set; }
        public long? AssetTierId { get; set; }
        public bool IsStartPackMember { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string FilesInfo { get; set; }

        public virtual AssetTier AssetTier { get; set; }
        public virtual AssetStoreInfo AssetStoreInfo { get; set; }
        public virtual CameraFilterCategory CameraFilterCategory { get; set; }
        public virtual CameraFilterSubCategory CameraFilterSubCategory { get; set; }
        public virtual ColorFilterCategory ColorFilterCategory { get; set; }
        public virtual Group Group { get; set; }
        public virtual Readiness Readiness { get; set; }
        public virtual LensFilterCategory LensFilterCategory { get; set; }
        public virtual User UploaderUser { get; set; }
        public virtual ICollection<AccountTransactionAndCameraFilter> AccountTransactionAndCameraFilter { get; set; }
        public virtual ICollection<BundleAndCameraFilter> BundleAndCameraFilter { get; set; }
        public virtual ICollection<DeviceTypeAndCameraFilter> DeviceTypeAndCameraFilter { get; set; }
        public virtual ICollection<GroupAndCameraFilter> GroupAndCameraFilter { get; set; }
        public virtual ICollection<PriceruleAndCameraFilter> PriceruleAndCameraFilter { get; set; }
        public virtual ICollection<CameraFilterVariant> CameraFilterVariant { get; set; }
        public virtual ICollection<QaComments> QaComments { get; set; }
        public virtual ICollection<SeasonAndCameraFilter> SeasonAndCameraFilter { get; set; }
        public virtual ICollection<WardrobeAndCameraFilter> WardrobeAndCameraFilter { get; set; }
    }
}