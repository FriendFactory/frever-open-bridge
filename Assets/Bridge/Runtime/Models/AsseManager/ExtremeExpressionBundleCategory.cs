﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class ExtremeExpressionBundleCategory
    {
        public ExtremeExpressionBundleCategory()
        {
            ExtremeExpressionBundle = new HashSet<ExtremeExpressionBundle>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ExtremeExpressionBundle> ExtremeExpressionBundle { get; set; }
    }
}