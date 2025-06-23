using System;
using Bridge.EnvironmentCompatibility;
using UnityEngine;

namespace Bridge
{
    internal sealed class VersionProvider
    {
        private const string VERSION_LOCATION = "Version";

        private string _bridgeVersion;
        private string _apiVersion;
        
        public string BridgeVersion
        {
            get
            {
                if (_bridgeVersion != null)
                    return _bridgeVersion;

                var versionInfo = LoadData();
                _bridgeVersion = versionInfo.Version;
                return _bridgeVersion;
            }
        }

        public string ApiVersion
        {
            get
            {
                if (_apiVersion != null)
                    return _apiVersion;

                var versionInfo = LoadData();
                var parsed = Version.Parse(versionInfo.Version);
                _apiVersion = $"{parsed.Major}.{parsed.Minor}";
                return _apiVersion;
            }
        }

        private BridgeVersion LoadData()
        {
            return Resources.Load<BridgeVersion>(VERSION_LOCATION);
        }
    }
}