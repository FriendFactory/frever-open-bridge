﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class PriceruleAndVerticalCategory
    {
        public long PriceruleId { get; set; }
        public long VerticalCategoryId { get; set; }

        public virtual Pricerule Pricerule { get; set; }
        public virtual VerticalCategory VerticalCategory { get; set; }
    }
}