namespace Bridge.Models.Common.Files
{
    public enum FileState
    {
        ModifiedLocally,
        PreUploaded,
        Uploading,
        SyncedWithServer,
        ShouldBeCopiedFromSource,
    }
}