﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class StickerSize
    {
        public StickerSize()
        {
            Sticker = new HashSet<Sticker>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Sticker> Sticker { get; set; }
    }
}