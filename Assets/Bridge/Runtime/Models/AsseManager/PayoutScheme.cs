﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class PayoutScheme
    {
        public PayoutScheme()
        {
            PayoutContract = new HashSet<PayoutContract>();
            PayoutSchemeRevShare = new HashSet<PayoutSchemeRevShare>();
        }

        public long Id { get; set; }
        public string DisplayName { get; set; }
        public int MinGuaranteeAmount { get; set; }

        public virtual ICollection<PayoutContract> PayoutContract { get; set; }
        public virtual ICollection<PayoutSchemeRevShare> PayoutSchemeRevShare { get; set; }
    }
}