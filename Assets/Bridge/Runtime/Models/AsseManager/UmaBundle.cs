﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class UmaBundle
    {
        public UmaBundle()
        {
            UmaAsset = new HashSet<UmaAsset>();
            UmaBundleAllDependencyDependsOnBundle = new HashSet<UmaBundleAllDependency>();
            UmaBundleAllDependencyUmaBundle = new HashSet<UmaBundleAllDependency>();
            UmaBundleDirectDependencyDependsOnBundle = new HashSet<UmaBundleDirectDependency>();
            UmaBundleDirectDependencyUmaBundle = new HashSet<UmaBundleDirectDependency>();
            Wardrobe = new HashSet<Wardrobe>();
        }

        public long Id { get; set; }
        public string AssetBundleName { get; set; }
        public string AssetBundleHash { get; set; }
        public string EncryptedName { get; set; }
        public long ReadinessId { get; set; }
        public long UmaBundleTypeId { get; set; }
        public long GroupId { get; set; }
        public string FilesInfo { get; set; }
        public long SizeKb { get; set; }
        public long[] GenderIds { get; set; }

        public virtual Group Group { get; set; }
        public virtual UmaBundleType UmaBundleType { get; set; }
        public virtual Readiness Readiness { get; set; }
        public virtual ICollection<UmaAsset> UmaAsset { get; set; }
        public virtual ICollection<UmaBundleAllDependency> UmaBundleAllDependencyDependsOnBundle { get; set; }
        public virtual ICollection<UmaBundleAllDependency> UmaBundleAllDependencyUmaBundle { get; set; }
        public virtual ICollection<UmaBundleDirectDependency> UmaBundleDirectDependencyDependsOnBundle { get; set; }
        public virtual ICollection<UmaBundleDirectDependency> UmaBundleDirectDependencyUmaBundle { get; set; }
        public virtual ICollection<Wardrobe> Wardrobe { get; set; }
    }
}