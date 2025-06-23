using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Results;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Bridge.EnvironmentCompatibility
{
    internal sealed class CompatibilityService: ICompatibilityService
    {
        private const string END_POINT = "api/Client/SupportedVersions";

        private readonly Dictionary<FFEnvironment, string> _environmentsUrls;
        private readonly string _bridgeVersion;
        private readonly string _apiVersion;
        
        public CompatibilityService(string bridgeVersion, string apiVersion, Dictionary<FFEnvironment, string> environmentsUrls)
        {
            _environmentsUrls = environmentsUrls;
            _bridgeVersion = bridgeVersion;
            _apiVersion = apiVersion;
        }

        public async Task<ArrayResult<EnvironmentCompatibilityResult>> GetEnvironmentsCompatibilityData()
        {
            var environments = Enum.GetValues(typeof(FFEnvironment)).Cast<FFEnvironment>().ToArray();
            var tasks = new Task<EnvironmentCompatibilityResult>[environments.Length];
            for (var i = 0; i < environments.Length; i++)
            {
                var env = environments[i];
                tasks[i] = GetEnvironmentCompatibilityData(env);
            }

            await Task.WhenAll(tasks);
            
            var compatibilityData = new List<EnvironmentCompatibilityResult>();
            foreach (var task in tasks)
            {
                compatibilityData.Add(task.Result);
            }
            return new ArrayResult<EnvironmentCompatibilityResult>(compatibilityData.ToArray());
        }

        public async Task<EnvironmentCompatibilityResult> GetEnvironmentCompatibilityData(FFEnvironment environment)
        {
            var host = _environmentsUrls[environment].AddApiVersion(_apiVersion);
            var endPointUrl = Extensions.CombineUrls(host, END_POINT);
            var request = UnityWebRequest.Get(new Uri(endPointUrl));
            try
            {
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success) 
                    return new EnvironmentCompatibilityResult(environment, request.downloadHandler.text);

                var json = request.downloadHandler.text;
                var versions = JsonConvert.DeserializeObject<SupportedVersions>(json);
                var isCompatible = IsCompatible(_bridgeVersion, versions.BridgeMinVersion,
                    versions.BridgeMaxVersion);
                return new EnvironmentCompatibilityResult(environment,
                    isCompatible, versions);

            }
            catch (Exception e)
            {
                return e.GetType() == typeof(JsonReaderException) 
                    ? new EnvironmentCompatibilityResult(environment, false, null) 
                    : new EnvironmentCompatibilityResult(environment, e.Message);
            }
        }

        private bool IsCompatible(string targetVersion, string minVersion, string maxVersion)
        {
            var target = new Version(targetVersion);
            var min = new Version(minVersion);
            
            if (string.IsNullOrEmpty(maxVersion))
                return target >= min;
            
            var max = new Version(maxVersion);
            return target >= min && target <= max;
        }
    }
}