using System.Threading.Tasks;
using Bridge.Authorization.Models;

namespace ApiTests
{
    public abstract class AnonymousAccessedEndPointTest: ApiTestBase
    {
        protected override void Start()
        {
            //skip authorisation
            RunTestAsync();
        }

        protected override Task<ICredentials> GetCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}