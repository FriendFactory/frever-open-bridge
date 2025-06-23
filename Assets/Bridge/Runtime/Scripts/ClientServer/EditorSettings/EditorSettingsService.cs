using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer.EditorsSetting;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.EditorSettings
{
    internal interface IEditorSettingsService
    {
        Task<Result<EditorsSettings>> GetDefaultSettings(CancellationToken token);
        
        Task<Result<EditorsSettings>> GetRemixSettings(CancellationToken token);
        
        Task<Result<EditorsSettings>> GetSettingForTask(long taskId, CancellationToken token);
    }

    internal sealed class EditorSettingsService : ServiceBase, IEditorSettingsService
    {
        public EditorSettingsService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public Task<Result<EditorsSettings>> GetDefaultSettings(CancellationToken token)
        {
            var endPoint = $"default";
            return GetEditorSettings(endPoint, token);
        }

        public Task<Result<EditorsSettings>> GetRemixSettings(CancellationToken token)
        {
            var endPoint = $"remix";
            return GetEditorSettings(endPoint, token);
        }

        public Task<Result<EditorsSettings>> GetSettingForTask(long taskId, CancellationToken token)
        {
            var endPoint = $"by-task/{taskId}";
            return GetEditorSettings(endPoint, token);
        }

        private async Task<Result<EditorsSettings>> GetEditorSettings(string endPoint, CancellationToken token)
        {
            try
            {
                var url = ConcatUrl(Host, $"editor-settings/{endPoint}");
                return await SendRequestForSingleModel<EditorsSettings>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<EditorsSettings>.Cancelled();
            }
        }
    }
}