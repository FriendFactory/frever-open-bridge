using System.Collections.Generic;
using System.Linq;

namespace Bridge.Authorization.Configs
{
    internal sealed class AuthServerConfigurations
    {
        private readonly List<AuthConfig> _configs = new List<AuthConfig>
        {
            new AuthConfig(
                "https://dev.frever-api.com/", FFEnvironment.Develop),
            new AuthConfig(
                "https://content-prod.frever-api.com/", FFEnvironment.Production),
            new AuthConfig(
                "https://content-test.frever-api.com/", FFEnvironment.Test),
            new AuthConfig(
                "https://content-stage.frever-api.com/", FFEnvironment.Stage),
            new AuthConfig(
                "https://content-prod-us.frever-api.com/", FFEnvironment.ProductionUSA)
        };

        public AuthConfig GetConfig(FFEnvironment ffEnvironment)
        {
            return _configs.First(x => x.Environment == ffEnvironment);
        }
    }
}