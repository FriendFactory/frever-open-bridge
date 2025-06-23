namespace Bridge.Models.Common
{
    public interface IAssetCategory: ICategory
    {
        bool HasNew { get; }
    }
}