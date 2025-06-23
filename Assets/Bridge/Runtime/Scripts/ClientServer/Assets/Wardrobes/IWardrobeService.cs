using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Constants;
using Bridge.Models.ClientServer.Assets;
using Bridge.Models.ClientServer.StartPack;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.Assets.Wardrobes
{
    internal interface IWardrobeService
    {
        Task<ArrayResult<WardrobeCategoriesForGender>> GetWardrobeCategoriesPerGender(CancellationToken token = default);
        Task<Result<WardrobeFullInfo>> GetWardrobe(long id, CancellationToken token = default);
        Task<Result<WardrobeFullInfo>> GetFitGenderWardrobe(long targetWardrobeId, long genderId, CancellationToken token = default);

        Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, 
            long genderId = 0, AssetSorting sorting = AssetSorting.NewestFirst, AssetPriceFilter assetPriceFilter = AssetPriceFilter.None, long? taskId = null, long[] tags = null, long? themeCollectionId = null, CancellationToken token = default);

        Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(WardrobeFilter filter, CancellationToken token);
        
        Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryTypeId, long genderId, long? wardrobeCategoryId, long? wardrobeSubCategoryId, long? themeCollectionId = null, CancellationToken token = default);
        Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(MyWardrobeFilterModel filter, CancellationToken token = default);
        Task<Result<MyWardrobesListInfo>> GetMyWardrobeListInfo(long? wardrobeCategoryTypeId, long? genderId, long? wardrobeCategoryId, long? wardrobeSubCategoryId, long? themeCollectionId = null, CancellationToken token = default);
        
        Task<ArrayResult<WardrobeFullInfo>> GetWardrobeList(long genderId, string[] wardrobeNames, CancellationToken token = default);
    }

    internal sealed class WardrobeService: AssetServiceBase, IWardrobeService
    {
        public WardrobeService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<WardrobeCategoriesForGender>> GetWardrobeCategoriesPerGender(CancellationToken token)
        {
            try
            {
                var url = BuildUrl("wardrobe/categories/by-gender");
                return await SendRequestForListModels<WardrobeCategoriesForGender>(url, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<WardrobeCategoriesForGender>.Cancelled();
            }
        }

        public async Task<Result<WardrobeFullInfo>> GetWardrobe(long id, CancellationToken token = default)
        {
            try
            {
                var url = BuildUrl($"Wardrobe/{id}");
                return await SendRequestForSingleModel<WardrobeFullInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<WardrobeFullInfo>.Cancelled();
            }
        }

        public async Task<Result<WardrobeFullInfo>> GetFitGenderWardrobe(long targetWardrobeId, long genderId, CancellationToken token = default)
        {
            try
            {
                var url = BuildUrl($"Wardrobe/{targetWardrobeId}/for-gender/{genderId}");
                return await SendRequestForSingleModel<WardrobeFullInfo>(url, token);
            }
            catch (OperationCanceledException)
            {
                return Result<WardrobeFullInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryId = null,
            long? wardrobeSubCategoryId = null, long genderId = 0, AssetSorting sorting = AssetSorting.NewestFirst, AssetPriceFilter assetPriceFilter = AssetPriceFilter.None, long? taskId = null, long[] tags = null, long? themeCollectionId = null, CancellationToken token = default)
        {
            try
            {
                return await GetWardrobeListInternal(target, takeNext, takePrevious, wardrobeCategoryId, wardrobeSubCategoryId, genderId, sorting, assetPriceFilter, taskId, tags, themeCollectionId, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<WardrobeShortInfo>();
            }
        }

        public async Task<ArrayResult<WardrobeShortInfo>> GetWardrobeList(WardrobeFilter filter, CancellationToken token)
        {
            try
            {
                return await WardrobeListInternal(filter, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<WardrobeShortInfo>();
            }
        }

        public async Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(long? target, int takeNext, int takePrevious, long? wardrobeCategoryTypeId, long genderId, long? wardrobeCategoryId, long? wardrobeSubCategoryId, long? themeCollectionId = null, CancellationToken token = default)
        {
            try
            {
                var body = new MyWardrobeFilterModel
                {
                    Target = target,
                    TakeNext = takeNext,
                    TakePrevious = takePrevious,
                    GenderId = genderId,
                    WardrobeCategoryTypeId = wardrobeCategoryTypeId,
                    WardrobeCategoryId = wardrobeCategoryId,
                    WardrobeSubCategoryId = wardrobeSubCategoryId,
                    ThemeCollectionId = themeCollectionId
                };
                return await GetMyWardrobeListInternal(body, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<WardrobeShortInfo>();
            }
        }

        public async Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeList(MyWardrobeFilterModel filter, CancellationToken token = default)
        {
            try
            {
                return await GetMyWardrobeListInternal(filter, token);
            }
            catch (OperationCanceledException)
            {
                return new CanceledArrayResult<WardrobeShortInfo>();
            }
        }

        public async Task<Result<MyWardrobesListInfo>> GetMyWardrobeListInfo(long? wardrobeCategoryTypeId, long? genderId, long? wardrobeCategoryId,
            long? wardrobeSubCategoryId, long? themeCollectionId, CancellationToken token = default)
        {
            try
            {
                return await GetMyWardrobeListInfoInternal(wardrobeCategoryTypeId, genderId, wardrobeCategoryId, wardrobeSubCategoryId, themeCollectionId, token);
            }
            catch (OperationCanceledException)
            {
                return Result<MyWardrobesListInfo>.Cancelled();
            }
        }

        public async Task<ArrayResult<WardrobeFullInfo>> GetWardrobeList(long genderId, string[] wardrobeNames, CancellationToken token = default)
        {
            try
            {
                var url = BuildUrl("Wardrobe/by-name");
                var body = new
                {
                    GenderId = genderId,
                    Names = wardrobeNames
                };
                return await SendRequestForListModels<WardrobeFullInfo>(url, token, body);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<WardrobeFullInfo>.Cancelled();
            }
        }

        private async Task<SingleObjectResult<WardrobeFullInfo>> GetWardrobeInternal(long id, CancellationToken token = default)
        {
            var url = BuildUrl($"Wardrobe/{id}");
            var req = RequestHelper.CreateRequest(url, HTTPMethods.Get, true, true);
            var resp = await req.GetHTTPResponseAsync(token);
            if (!resp.IsSuccess)
            {
                return new SingleObjectResult<WardrobeFullInfo>(resp.DataAsText);
            }

            var wardrobe = Serializer.DeserializeProtobuf<WardrobeFullInfo>(resp.Data);
            return new SingleObjectResult<WardrobeFullInfo>(wardrobe);
        }

        private Task<ArrayResult<WardrobeShortInfo>> GetWardrobeListInternal(long? target, int takeNext, int takePrevious,
            long? wardrobeCategoryId = null, long? wardrobeSubCategoryId = null, long genderId = 0,
            AssetSorting sorting = AssetSorting.NewestFirst, AssetPriceFilter assetPriceFilter = AssetPriceFilter.None, 
            long? taskId = null, long[] tags = null, long? themeCollectionId = null, CancellationToken token = default)
        {
            var body = new WardrobeFilter
            {
                Target = target,
                TakeNext = takeNext,
                TakePrevious = takePrevious,
                WardrobeCategoryId = wardrobeCategoryId,
                WardrobeSubCategoryId = wardrobeSubCategoryId,
                GenderId = genderId,
                Sorting = sorting,
                PriceFilter = assetPriceFilter,
                TaskId = taskId,
                ThemeCollectionId = themeCollectionId,
                TagIds = tags,
                UnityVersion = UnityConstants.UnityVersion
            };
            return WardrobeListInternal(body, token);
        }

        private Task<ArrayResult<WardrobeShortInfo>> WardrobeListInternal(WardrobeFilter filter, CancellationToken token)
        {
            var url = BuildUrl("wardrobe");
            return SendRequestForListModels<WardrobeShortInfo>(url, token, filter);
        }

        private Task<ArrayResult<WardrobeShortInfo>> GetMyWardrobeListInternal(MyWardrobeFilterModel filter, CancellationToken token)
        {
            var url = BuildUrl("wardrobe/my");
            return SendRequestForListModels<WardrobeShortInfo>(url, token, filter);
        }
        
        private Task<Result<MyWardrobesListInfo>> GetMyWardrobeListInfoInternal(long? wardrobeCategoryTypeId, long? genderId, long? wardrobeCategoryId,
            long? wardrobeSubCategoryId, long? themeCollectionId, CancellationToken token = default)
        {
            var url = BuildUrl("wardrobe/my/info");
            var body = new
            {
                GenderId = genderId,
                WardrobeCategoryTypeId = wardrobeCategoryTypeId,
                WardrobeCategoryId = wardrobeCategoryId,
                WardrobeSubCategoryId = wardrobeSubCategoryId,
                ThemeCollectionId = themeCollectionId
            };
            return SendRequestForSingleModel<MyWardrobesListInfo>(url, token, false, body);
        }
    }

    public interface IWardrobeFilter
    {
        int TakePrevious { get; set; }
        int TakeNext { get; set; }
        long? Target { get; set; }
        long? WardrobeSubCategoryId { get; set; }
        long? WardrobeCategoryId { get; set; }
        long GenderId { get; set; }
    }
    
    public struct WardrobeFilter: IWardrobeFilter
    {
        public long? WardrobeSubCategoryId { get; set; }

        public long? WardrobeCategoryId { get; set; }

        public long GenderId { get; set; }

        public long? TaskId { get; set; }

        public long[] TagIds { get; set; }

        public AssetSorting Sorting { get; set; }

        public AssetPriceFilter PriceFilter { get; set; }

        public long? Target { get; set; }

        public int TakePrevious { get; set; }

        public int TakeNext { get; set; }
        
        public long? ThemeCollectionId { get; set; }
        
        public string UnityVersion { get; set; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is WardrobeFilter other))
            {
                return false;
            }

            return WardrobeSubCategoryId.Compare(other.WardrobeSubCategoryId)
                   && WardrobeCategoryId.Compare(other.WardrobeCategoryId)
                   && GenderId == other.GenderId
                   && TaskId.Compare(other.TaskId)
                   && ((TagIds == null && other.TagIds == null) ||
                       (TagIds != null && other.TagIds != null && TagIds.SequenceEqual(other.TagIds)))
                   && Sorting.Equals(other.Sorting)
                   && PriceFilter.Equals(other.PriceFilter)
                   && Target.Compare(other.Target)
                   && TakePrevious == other.TakePrevious
                   && TakeNext == other.TakeNext
                   && ThemeCollectionId.Compare(other.ThemeCollectionId)
                   && UnityVersion == other.UnityVersion;
        }
        
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + WardrobeSubCategoryId.GetHashCode();
                hash = hash * 23 + WardrobeCategoryId.GetHashCode();
                hash = hash * 23 + GenderId.GetHashCode();
                hash = hash * 23 + TaskId.GetHashCode();
                if (TagIds != null)
                {
                    foreach (var tagId in TagIds)
                    {
                        hash = hash * 23 + tagId.GetHashCode();
                    }
                }
                hash = hash * 23 + Sorting.GetHashCode();
                hash = hash * 23 + PriceFilter.GetHashCode();
                hash = hash * 23 + Target.GetHashCode();
                hash = hash * 23 + TakePrevious.GetHashCode();
                hash = hash * 23 + TakeNext.GetHashCode();
                hash = hash * 23 + ThemeCollectionId.GetHashCode();
                return hash;
            }
        }
    }
    
    public struct MyWardrobeFilterModel: IWardrobeFilter
    {
        public long? WardrobeCategoryTypeId { get; set; }

        public long? WardrobeSubCategoryId { get; set; }

        public long? WardrobeCategoryId { get; set; }

        public long GenderId { get; set; }

        public long? Target { get; set; }

        public int TakePrevious { get; set; }

        public int TakeNext { get; set; }
      
        public long? ThemeCollectionId { get; set; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is MyWardrobeFilterModel other))
            {
                return false;
            }

            return WardrobeCategoryTypeId.Compare(other.WardrobeCategoryTypeId)
                   && WardrobeSubCategoryId.Compare(other.WardrobeSubCategoryId)
                   && WardrobeCategoryId.Compare(other.WardrobeCategoryId)
                   && GenderId == other.GenderId
                   && Target.Compare(other.Target)
                   && TakePrevious == other.TakePrevious
                   && TakeNext == other.TakeNext
                   &&  ThemeCollectionId.Compare(other.ThemeCollectionId);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + WardrobeCategoryTypeId.GetHashCode();
                hash = hash * 23 + WardrobeSubCategoryId.GetHashCode();
                hash = hash * 23 + WardrobeCategoryId.GetHashCode();
                hash = hash * 23 + GenderId.GetHashCode();
                hash = hash * 23 + Target.GetHashCode();
                hash = hash * 23 + TakePrevious.GetHashCode();
                hash = hash * 23 + TakeNext.GetHashCode();
                return hash;
            }
        }
    }
}