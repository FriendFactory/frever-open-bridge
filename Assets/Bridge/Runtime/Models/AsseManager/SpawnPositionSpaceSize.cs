﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class SpawnPositionSpaceSize
    {
        public SpawnPositionSpaceSize()
        {
            CharacterSpawnPosition = new HashSet<CharacterSpawnPosition>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int MaxFov { get; set; }
        public int MaxDistance { get; set; }
        public int MinFov { get; set; }
        public int MinDistance { get; set; }
        public int StartFov { get; set; }
        public int StartDistance { get; set; }

        public virtual ICollection<CharacterSpawnPosition> CharacterSpawnPosition { get; set; }
    }
}