using System.Collections.Generic;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

namespace ApiTests.VideoClipTests
{
    public class CreatePhotoTest: EntityApiTest<Photo>
    {
        protected override async void RunTestAsync()
        {
            var photo = new Photo();

            var filePath = GetFilePath(TestFileNames.IMAGE_JPG);
            var fileInfo = new FileInfo(filePath, FileType.MainFile);

            photo.Files = new List<FileInfo> {fileInfo};

            await Bridge.PostAsync(photo);
        }
    }
}