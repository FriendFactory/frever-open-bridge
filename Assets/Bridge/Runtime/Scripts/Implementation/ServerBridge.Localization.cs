using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization.Configs;
using Bridge.ClientServer.Localization;

namespace Bridge
{
    public partial class ServerBridge
    {
        public bool HasCached(string isoCode, out string path)
        {
            return _localizationService.HasCached(isoCode, out path);
        }

        public Task<LocalizationDataResult> GetLocalizationData(string isoCode, CancellationToken token = default)
        {
            SetupAuthBridge(Environment);
            SetupLocalizationService();
            return _localizationService.GetLocalizationData(isoCode, token);
        }
                
        private void SetupLocalizationService()
        {
            FFEnvironment environment;
 
            if(!_authService.IsLoggedIn && LastLoggedEnvironment.HasValue)
            {
                environment = LastLoggedEnvironment.Value;
            }
            else
            {
                environment = Environment;
            }
            
            var config = _authServerConfigurations.GetConfig(environment);
            
            if (_localizationService != null && _localizationService.Environment == config.Environment) return;
            
            var url = $"{config.Host}{ApiVersion}/client/api/";
            _localizationService = new LocalizationService(url, 
                _requestHelper, 
                _serializer, 
                ROOT_CACHE_FOLDER, 
                config.Environment);
        }
    }
}