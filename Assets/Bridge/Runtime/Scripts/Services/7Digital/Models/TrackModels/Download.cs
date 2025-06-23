using System;
using System.Collections.Generic;

namespace Bridge.Services._7Digital.Models
{
    internal sealed class Download
    {
        public DateTime releaseDate { get; set; }
        public DateTime previewDate { get; set; }
        public List<Package> packages { get; set; }
    }
}
