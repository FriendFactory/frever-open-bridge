using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Models.Common.Files;
using UnityEngine;
using Event = Bridge.Models.AsseManager.Event;
using FileInfo = Bridge.Models.Common.Files.FileInfo;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.Events
{
    public class UpdateOnlyEventThumbnail: EntityApiTest<Event>
    {
        protected override async void RunTestAsync()
        {
            var eventId = await GetAnyAvailableEntityId<Event>();
             
            var fileName = GetFilePath(TestFileNames.THUMBNAIL_PNG_1);
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
            var resolution = Resolution._512x512;
            var fileInfo = new FileInfo(fullPath, FileType.Thumbnail, resolution);

            await SaveEventThumbnail(eventId, fileInfo);
        }
        
        private async Task SaveEventThumbnail(long eventId, FileInfo thumbnailFileInfo, Action<Event> onSuccess = null, Action onFailure = null)
        {
            var @event = new Event
            {
                Id = eventId,
                Files = new List<FileInfo>{thumbnailFileInfo}
            };
            var updateRequest = new PrimitiveFieldsUpdateReq<Event>(@event);
            updateRequest.UpdateProperty(nameof(Event.Files));
            var updateEventTask = Bridge.UpdateAsync(updateRequest, true);
            await updateEventTask;
            if (updateEventTask.Result.IsSuccess)
            {
                onSuccess?.Invoke(updateEventTask.Result.ResultObject);
            }
            if (updateEventTask.Result.IsError)
            {
                Debug.LogError($"Failed to update properties {nameof(Event)}. Reason: {updateEventTask.Result.ErrorMessage}");
                onFailure?.Invoke();
            }
        }
    }
}