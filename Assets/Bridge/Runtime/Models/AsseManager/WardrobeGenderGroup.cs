﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class WardrobeGenderGroup
    {
        public WardrobeGenderGroup()
        {
            Wardrobe = new HashSet<Wardrobe>();
        }

        public long Id { get; set; }

        public Guid GlobalId { get; set; }

        public virtual ICollection<Wardrobe> Wardrobe { get; set; }
    }
}