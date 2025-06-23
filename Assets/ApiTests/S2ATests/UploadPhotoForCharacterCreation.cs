using System.IO;
using ApiTests;

public class UploadPhotoForCharacterCreation : AuthorizedUserApiTestBase
{
    protected override async void RunTestAsync()
    {
        var photoPath = GetFilePath(TestFileNames.SELFIE_HEIC);
        var bytes = File.ReadAllBytes(photoPath);
        var resp = await Bridge.GetSelfieJsonAsync(bytes, "male", 0.4f);
    }
}
