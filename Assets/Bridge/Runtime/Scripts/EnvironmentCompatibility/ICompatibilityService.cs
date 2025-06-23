using System.Threading.Tasks;
using Bridge.Results;

namespace Bridge.EnvironmentCompatibility
{
    public interface ICompatibilityService
    {
        Task<ArrayResult<EnvironmentCompatibilityResult>> GetEnvironmentsCompatibilityData();
        
        Task<EnvironmentCompatibilityResult> GetEnvironmentCompatibilityData(FFEnvironment environment);
    }
}