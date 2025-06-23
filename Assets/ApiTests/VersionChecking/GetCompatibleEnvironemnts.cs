using System.Threading.Tasks;
using ApiTests;
using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

public class GetCompatibleEnvironemnts : ApiTestBase
{
    protected override async void RunTestAsync()
    {
       var environmentsData = await Bridge.GetEnvironmentsCompatibilityData();
       Debug.Log(JsonConvert.SerializeObject(environmentsData.Models));
    }

    protected override void Start()
    {
        Bridge.ChangeEnvironment(Environment);
        RunTestAsync();
    }
    
    protected override Task<ICredentials> GetCredentials()
    {
        throw new System.NotImplementedException();
    }
}
