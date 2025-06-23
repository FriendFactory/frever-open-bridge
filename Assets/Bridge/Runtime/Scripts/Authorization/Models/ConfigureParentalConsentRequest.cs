using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Authorization.Models
{
    public class ConfigureParentalConsentRequest
    {
        public ParentalConsent ParentalConsent { get; set; }
    }
    
    public class ParentalConsent
    {
        public bool AllowChat { get; set; }             /* Checked */
        public bool AllowComments { get; set; }         /* Checked */
        public bool AllowVideoDescription { get; set; } /* Checked */
        public bool AllowCaptions { get; set; }         /* Checked */
        public bool AudioUploads { get; set; }          /* Checked */
        public bool VideoUploads { get; set; }          /* Checked */
        public bool ImageUploads { get; set; }
        public bool AccessMicrophone { get; set; }
        public bool AccessCamera { get; set; }
        public bool ShareContacts { get; set; }
        public bool PushNotifications { get; set; }
        public bool AllowCrewCreation { get; set; }
        [ProtoNewField(1)] public bool AllowInAppPurchase { get; set; } = true;
    }
}