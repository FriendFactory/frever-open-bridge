using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bridge.Results;

namespace Bridge.Services.Advertising
{
    public sealed class SongsAdResult: Result
    {
        public ReadOnlyCollection<SongAdData> SongAdData;

        internal SongsAdResult(IList<SongAdData> data)
        {
            SongAdData = new ReadOnlyCollection<SongAdData>(data);
        }

        internal SongsAdResult(string error):base(error)
        {
            
        }
    }
}