﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BodyAnimationAndCharacterSpawnPosition
    {
        public long BodyAnimationId { get; set; }
        public long CharacterSpawnPositionId { get; set; }

        public virtual AsseManager.BodyAnimation BodyAnimation { get; set; }
        public virtual CharacterSpawnPosition CharacterSpawnPosition { get; set; }
    }
}