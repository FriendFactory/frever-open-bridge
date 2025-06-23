using Bridge.Results;
using Bridge.Services._7Digital.Models;
using Bridge.Services._7Digital.Models.TrackModels;

namespace Bridge.Services._7Digital
{
    public sealed class TracksDetailsResult: Result
    {
        public readonly ExternalTrackInfo[] Models;
        public readonly TrackError[] TrackErrors;
        
        private static TracksDetailsResult _cachedCancelledResult;

        private TracksDetailsResult(ExternalTrackInfo[] tracks, TrackError[] trackErrors)
        {
            Models = tracks;
            TrackErrors = trackErrors;
        }

        private TracksDetailsResult(string errorMessage, int? statusCode = null) : base(errorMessage, statusCode)
        {
        }

        private TracksDetailsResult(bool isCanceled) : base(true)
        {
        }

        public static TracksDetailsResult Success(ExternalTrackInfo[] models, TrackError[] trackErrors)
        {
            return new TracksDetailsResult(models, trackErrors);
        }

        public static TracksDetailsResult Error(string error)
        {
            return new TracksDetailsResult(error);
        }

        public static TracksDetailsResult Cancelled()
        {
            return _cachedCancelledResult ?? (_cachedCancelledResult = new TracksDetailsResult(true));
        }
    }
}