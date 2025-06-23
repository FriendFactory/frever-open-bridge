using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bridge.Models.Common.Files;

namespace Bridge.Results
{
    public sealed class UpdateFilesResult: Result
    {
        public readonly IReadOnlyDictionary<long, List<FileInfo>> UpdatedEventFilesData;

        internal UpdateFilesResult(IDictionary<long, List<FileInfo>> updatedEventThumbnailsData)
        {
            UpdatedEventFilesData = new ReadOnlyDictionary<long, List<FileInfo>>(updatedEventThumbnailsData);
        }

        internal UpdateFilesResult(string errorMessage, int? statusCode): base(errorMessage, statusCode)
        {
        }
    }
}