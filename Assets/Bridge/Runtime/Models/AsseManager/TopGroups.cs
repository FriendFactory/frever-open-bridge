﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class TopGroups
    {
        public long GroupId { get; set; }
        public int SortKey { get; set; }
        public int NumFollowing { get; set; }
        public int NumFollowers { get; set; }
        public int NumPosts { get; set; }
        public int NumLikes { get; set; }

        public virtual Group Group { get; set; }
    }
}