﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class Follower
    {
        public long FollowingId { get; set; }
        public long FollowerId { get; set; }

        public virtual Group FollowerNavigation { get; set; }
        public virtual Group Following { get; set; }
    }
}