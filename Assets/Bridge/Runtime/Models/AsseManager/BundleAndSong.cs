﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BundleAndSong
    {
        public long BundleId { get; set; }
        public long SongId { get; set; }

        public virtual Bundle Bundle { get; set; }
        public virtual Song Song { get; set; }
    }
}