namespace Bridge.Models.Common
{
    public interface ICategoryMember
    {
        long CategoryId { get; }
        ICategory Category { get; }
    }
}