﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class Language
    {
        public Language()
        {
            Level = new HashSet<Level>();
            VoiceTrack = new HashSet<VoiceTrack>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Level> Level { get; set; }
        public virtual ICollection<VoiceTrack> VoiceTrack { get; set; }
    }
}