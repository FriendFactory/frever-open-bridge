﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class CampaignFulfillment
    {
        public long GroupId { get; set; }
        public long CampaignId { get; set; }
        public DateTime FulfillmentTime { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Group Group { get; set; }
    }
}