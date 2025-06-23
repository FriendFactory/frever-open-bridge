using System.Threading.Tasks;
using Bridge.EnvironmentCompatibility;
using Bridge.Results;

namespace Bridge
{
    public interface IEnvironmentInfo
    {
        FFEnvironment? LastLoggedEnvironment { get; }
        FFEnvironment Environment { get; }
    }
    
    public interface IEnvironmentControl: IEnvironmentInfo
    {
        void ChangeEnvironment(FFEnvironment next);
        Task<ArrayResult<EnvironmentCompatibilityResult>> GetEnvironmentsCompatibilityData();
    }
}