﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class WardrobeAndWardrobeSubCategory
    {
        public long WardrobeId { get; set; }
        public long WardrobeSubCategoryId { get; set; }
        public int SubCategorySortOrder { get; set; }

        public virtual Wardrobe Wardrobe { get; set; }
        public virtual WardrobeSubCategory WardrobeSubCategory { get; set; }
    }
}