using System;

namespace Bridge.Services.SelfieAvatar.JSONStructs
{
    [Serializable]
    public struct UMAPredictions 
    {
        public int gender;
        public int ethnicity;
        public FaceAttribute faceAttributes;
        public HairAttribute hairAttributes;
        public bool hasGlasses;
        public SelfieColor skinColor;
    }
}
