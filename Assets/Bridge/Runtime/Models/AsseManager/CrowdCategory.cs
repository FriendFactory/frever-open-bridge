﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class CrowdCategory
    {
        public CrowdCategory()
        {
            Crowd = new HashSet<Crowd>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Crowd> Crowd { get; set; }
    }
}