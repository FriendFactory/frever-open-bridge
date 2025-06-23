using System.Collections.Generic;

namespace Bridge.Services._7Digital.Models
{
    internal sealed class SearchResults
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalItems { get; set; }
        public List<SearchResult> searchResult { get; set; }
    }
}
