namespace Bridge.Models.Common
{
    public interface INewTrackable: IEntity
    {
        bool IsNew { get; }
    }

    public interface IPurchasable
    {
        AssetOfferInfo AssetOffer { get; }
    }

    public interface IMinLevelRequirable
    {
        long? SeasonLevel { get; }
    }

    public interface ICategorizable
    {
        long CategoryId { get; }
    }
    
    public interface ISubCategorizable
    {
        long[] SubCategories { get; }
    }
}