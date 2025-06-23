using System;
using System.Collections.Generic;

namespace Bridge.Services._7Digital.Models.PlaylistModels
{
    internal sealed class Playlist
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<PlaylistTrack> tracks { get; set; }
    }
}
