using System.Collections.Generic;

namespace Bridge.Services._7Digital.Models
{
    internal sealed class Package 
    {
        public int id { get; set; }
        public string description { get; set; }
        public Price price { get; set; }
        public List<Format> formats { get; set; }
    }
}
