﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class AccountBalance
    {
        public long Id { get; set; }
        public long BalancePaidfor { get; set; }
        public long BalanceGiven { get; set; }

        public virtual Group IdNavigation { get; set; }
    }
}