using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer
{
    internal interface ICountryService
    {
        Task<ArrayResult<CountryInfo>> GetCountriesListAsync(CancellationToken token);

        Task<Result<CountryInfo>> GetCountryInfoAsync(string isoCode, CancellationToken token);
    }
    
    internal sealed class CountryService: ServiceBase, ICountryService
    {
        public CountryService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public async Task<ArrayResult<CountryInfo>> GetCountriesListAsync(CancellationToken token)
        {
            try
            {
                return await GetCountriesListAsyncInternal(token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<CountryInfo>.Cancelled();
            }
        }

        public async Task<Result<CountryInfo>> GetCountryInfoAsync(string isoCode, CancellationToken token)
        {
            try
            {
                return await GetCountryInfoAsyncInternal(isoCode, token);
            }
            catch (OperationCanceledException)
            {
                return Result<CountryInfo>.Cancelled();
            }
        }

        private Task<ArrayResult<CountryInfo>> GetCountriesListAsyncInternal(CancellationToken token)
        {
            var url = ConcatUrl(Host, "country");
            return SendRequestForListModels<CountryInfo>(url, token);
        }
        
        private Task<Result<CountryInfo>> GetCountryInfoAsyncInternal(string isoCode, CancellationToken token)
        {
            var url = ConcatUrl(Host, $"country/{isoCode}");
            return SendRequestForSingleModel<CountryInfo>(url, token);
        }
    }
}