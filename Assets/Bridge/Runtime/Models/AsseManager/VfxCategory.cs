﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class VfxCategory
    {
        public VfxCategory()
        {
            BrandAndVfxCategory = new HashSet<BrandAndVfxCategory>();
            SetLocationCategoryAndVfxCategory = new HashSet<SetLocationCategoryAndVfxCategory>();
            SetLocationMoodAndVfxCategory = new HashSet<SetLocationMoodAndVfxCategory>();
            Vfx = new HashSet<Vfx>();
            VfxCategoryAndGenre = new HashSet<VfxCategoryAndGenre>();
            WardrobeCollectionAndVfxCategory = new HashSet<WardrobeCollectionAndVfxCategory>();
            WardrobeStyleAndVfxCategory = new HashSet<WardrobeStyleAndVfxCategory>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public virtual ICollection<BrandAndVfxCategory> BrandAndVfxCategory { get; set; }
        public virtual ICollection<SetLocationCategoryAndVfxCategory> SetLocationCategoryAndVfxCategory { get; set; }
        public virtual ICollection<SetLocationMoodAndVfxCategory> SetLocationMoodAndVfxCategory { get; set; }
        public virtual ICollection<Vfx> Vfx { get; set; }
        public virtual ICollection<VfxCategoryAndGenre> VfxCategoryAndGenre { get; set; }
        public virtual ICollection<WardrobeCollectionAndVfxCategory> WardrobeCollectionAndVfxCategory { get; set; }
        public virtual ICollection<WardrobeStyleAndVfxCategory> WardrobeStyleAndVfxCategory { get; set; }
    }
}