﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BrandAndGenre
    {
        public long BrandId { get; set; }
        public long GenreId { get; set; }
        public int BrandSortKey { get; set; }
        public int GenreSortKey { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Genre Genre { get; set; }
    }
}