﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class PaymentSystem
    {
        public PaymentSystem()
        {
            ExchangeTransaction = new HashSet<ExchangeTransaction>();
        }

        public long Id { get; set; }
        public bool Active { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<ExchangeTransaction> ExchangeTransaction { get; set; }
    }
}