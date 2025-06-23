using System.Threading.Tasks;
using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.VersionChecking
{
    public class GetEnvironmentCompatibility : ApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var environmentsData = await Bridge.GetEnvironmentCompatibilityData(Environment);
            Debug.Log(JsonConvert.SerializeObject(environmentsData));
        }

        protected override void Start()
        {
            RunTestAsync();
        }
        
        protected override Task<ICredentials> GetCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}