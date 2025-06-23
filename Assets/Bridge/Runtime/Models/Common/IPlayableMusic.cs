namespace Bridge.Models.Common
{
    public interface IPlayableMusic: IFilesAttachedEntity
    {
        int Duration { get; set; }
    }
}
