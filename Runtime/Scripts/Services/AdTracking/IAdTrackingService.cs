using System;
using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.Services.AdTracking
{
    public interface IAdTrackingService
    {
        Task<Result> AddTrackingId(string id);
        Task<Result> RemoveAllTrackingIds();
    }
}