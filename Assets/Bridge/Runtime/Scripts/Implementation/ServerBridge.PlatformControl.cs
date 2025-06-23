using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common.Files;
using UnityEngine;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private static readonly RuntimePlatform[] SupportedPlatforms = {RuntimePlatform.IPhonePlayer, RuntimePlatform.Android};

        private RuntimePlatform _runtimePlatform = SupportedPlatforms.First();
        private Platform Platform => _platformsMap[_runtimePlatform];

        private readonly Dictionary<RuntimePlatform, Platform> _platformsMap = new Dictionary<RuntimePlatform, Platform>()
        {
            {RuntimePlatform.IPhonePlayer, Platform.iOS},
            {RuntimePlatform.Android, Platform.Android}
        };

        public void SetPlatform(RuntimePlatform platform)
        {
            if (!SupportedPlatforms.Contains(platform))
            {
                throw new InvalidOperationException($"{platform} platform is not supported");
            }
            _runtimePlatform = platform;
        }
    }
}