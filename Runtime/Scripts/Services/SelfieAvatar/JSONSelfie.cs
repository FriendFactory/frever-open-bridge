using System;
using Bridge.Services.SelfieAvatar.JSONStructs;

namespace Bridge.Services.SelfieAvatar
{
    [Serializable]
    public struct JSONSelfie 
    {
        public string image_id;
        public UMAPredictions predictions;
        public float request_time;
        public string request_status;
        public bool success;
    }
}
