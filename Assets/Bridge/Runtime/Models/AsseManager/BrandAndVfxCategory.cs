﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BrandAndVfxCategory
    {
        public long BrandId { get; set; }
        public long VfxCategoryId { get; set; }
        public int BrandSortKey { get; set; }
        public int VfxCategorySortKey { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual VfxCategory VfxCategory { get; set; }
    }
}