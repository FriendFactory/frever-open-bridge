using System;

namespace Bridge.Services.SelfieAvatar.JSONStructs
{
    [Serializable]
    public struct HairAttribute 
    {
        public int facialHairType;
        public string hairCode;
        public SelfieColor hairColor;
        
        public int hairCurve;
        public int hairLength;
        public short hairBangs;
    }
}
