﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class Font
    {
        public Font()
        {
            StickerController = new HashSet<StickerController>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Caption> Caption { get; set; }
        public virtual ICollection<StickerController> StickerController { get; set; }
    }
}