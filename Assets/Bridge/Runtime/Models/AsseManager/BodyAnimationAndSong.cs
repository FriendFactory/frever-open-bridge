﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BodyAnimationAndSong
    {
        public long BodyAnimationId { get; set; }
        public long SongId { get; set; }

        public virtual AsseManager.BodyAnimation BodyAnimation { get; set; }
        public virtual Song Song { get; set; }
    }
}