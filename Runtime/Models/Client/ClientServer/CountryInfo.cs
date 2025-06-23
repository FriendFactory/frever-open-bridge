using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Models.ClientServer
{
    public class CountryInfo
    {
        public long Id { get; set; }

        public string Iso2Code { get; set; }

        public string Iso3Code { get; set; }

        public bool EnableMusic { get; set; }
        
        [ProtoNewField(1)] public int AgeOfConsent { get; set; }
    }
}