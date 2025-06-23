using System;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.Assets
{
    public class LightSettingsFullInfo: IEntity, IUnityGuid
    {
        public long Id { get; set; }

        public string Color { get; set; }

        public int Intensity { get; set; }

        public Guid UnityGuid { get; set; }
    }
}