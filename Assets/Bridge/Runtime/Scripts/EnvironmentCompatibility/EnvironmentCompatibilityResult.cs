using Bridge.Results;

namespace Bridge.EnvironmentCompatibility
{
    public sealed class EnvironmentCompatibilityResult: Result
    {
        public readonly FFEnvironment Environment;
        public readonly bool IsCompatibleWithBridge;
        public readonly SupportedVersions SupportedVersions;

        public EnvironmentCompatibilityResult(
            FFEnvironment environment,
            bool isCompatibleWithBridge, 
            SupportedVersions supportedVersions)
        {
            Environment = environment;
            IsCompatibleWithBridge = isCompatibleWithBridge;
            SupportedVersions = supportedVersions;
        }

        public EnvironmentCompatibilityResult(FFEnvironment environment,
            string errorMessage) : base(errorMessage)
        {
            Environment = environment;
        }
    }
}